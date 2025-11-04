// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace Evoogle.Logging;

/// <summary>
///     A flexible implementation of <see cref="ILogger"/> that can forward log messages to multiple outputs depending on configuration.
///
///     <para>This logger supports:</para>
///     <list type="bullet">
///         <item><description><see cref="ILogger"/> forwarding, if injected</description></item>
///         <item><description><see cref="Debug.WriteLine(string)"/> when debugging</description></item>
///         <item><description><see cref="Console.WriteLine(string)"/> for command-line diagnostics or visibility</description></item>
///     </list>
///
///     <para>
///         Configure the desired behavior via <see cref="MultiplexingLoggerMode"/>.
///     </para>
/// </summary>
/// <remarks>
///     Use <see cref="MultiplexingLoggerMode"/> to configure one or more targets.
///     This logger is especially useful in test utilities, converters, or diagnostics where structured logging may not be fully wired up.
/// </remarks>
/// <remarks>
///     Initializes a new instance of the <see cref="MultiplexingLogger"/> class.
/// </remarks>
/// <param name="innerLogger">An optional <see cref="ILogger"/> to forward log messages to.</param>
/// <param name="mode">Specifies one or more log targets for output.</param>
public sealed class MultiplexingLogger(ILogger? innerLogger = null, MultiplexingLoggerMode mode = MultiplexingLoggerMode.Logger) : ILogger
{
    #region Types
    /// <summary>Represents a no-op scope used when no logger is available.</summary>
    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose()
        { }
    }
    #endregion

    #region Fields
    private readonly ILogger? _innerLogger = innerLogger;
    private readonly MultiplexingLoggerMode _mode = mode;
    #endregion

    #region ILogger Methods
    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull => _innerLogger?.BeginScope(state) ?? NullScope.Instance;

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        if (_innerLogger?.IsEnabled(logLevel) == true)
        {
            return true;
        }

        return (_mode.HasFlag(MultiplexingLoggerMode.Debug) && Debugger.IsAttached) || _mode.HasFlag(MultiplexingLoggerMode.Console);
    }

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!this.IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        var levelName = logLevel.ToString().ToUpperInvariant().PadRight(7); // e.g. "WARNING"

        if (_mode.HasFlag(MultiplexingLoggerMode.Logger) && _innerLogger is not null)
        {
            _innerLogger.Log(logLevel, eventId, state, exception, formatter);
        }

        if (_mode.HasFlag(MultiplexingLoggerMode.Debug) && Debugger.IsAttached)
        {
            Debug.WriteLine($"[{levelName}] {message}");

            if (exception is not null)
            {
                Debug.WriteLine(exception);
            }
        }

        if (_mode.HasFlag(MultiplexingLoggerMode.Console))
        {
            Console.WriteLine($"[{levelName}] {message}");

            if (exception is not null)
            {
                Console.WriteLine(exception);
            }
        }
    }
    #endregion
}
