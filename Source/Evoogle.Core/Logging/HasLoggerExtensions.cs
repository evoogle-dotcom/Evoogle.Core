// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

namespace Evoogle.Logging;

/// <summary>
///     Extension methods for <see cref="IHasLogger"/> that forward log calls to the underlying <see cref="ILogger"/>.
/// </summary>
[SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "Forwarding wrapper to Microsoft.Extensions.Logging.LoggerExtensions.")]
[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "Forwarding wrapper; delegates not applicable for arbitrary templates.")]
public static class HasLoggerExtensions
{
    //------------------------------------------DEBUG------------------------------------------//

    /// <summary>Logs a debug-level message with an event ID, exception, and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogDebug(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogDebug(hasLogger.Logger, eventId, exception, message, args);

    /// <summary>Logs a debug-level message with an event ID and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogDebug(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogDebug(hasLogger.Logger, eventId, message, args);

    /// <summary>Logs a debug-level message with an exception and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogDebug(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogDebug(hasLogger.Logger, exception, message, args);

    /// <summary>Logs a debug-level formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogDebug(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogDebug(hasLogger.Logger, message, args);

    //------------------------------------------TRACE------------------------------------------//

    /// <summary>Logs a trace-level message with an event ID, exception, and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogTrace(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogTrace(hasLogger.Logger, eventId, exception, message, args);

    /// <summary>Logs a trace-level message with an event ID and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogTrace(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogTrace(hasLogger.Logger, eventId, message, args);

    /// <summary>Logs a trace-level message with an exception and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogTrace(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogTrace(hasLogger.Logger, exception, message, args);

    /// <summary>Logs a trace-level formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogTrace(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogTrace(hasLogger.Logger, message, args);

    //------------------------------------------INFORMATION------------------------------------------//

    /// <summary>Logs an information-level message with an event ID, exception, and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogInformation(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogInformation(hasLogger.Logger, eventId, exception, message, args);

    /// <summary>Logs an information-level message with an event ID and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogInformation(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogInformation(hasLogger.Logger, eventId, message, args);

    /// <summary>Logs an information-level message with an exception and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogInformation(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogInformation(hasLogger.Logger, exception, message, args);

    /// <summary>Logs an information-level formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogInformation(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogInformation(hasLogger.Logger, message, args);

    //------------------------------------------WARNING------------------------------------------//

    /// <summary>Logs a warning-level message with an event ID, exception, and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogWarning(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogWarning(hasLogger.Logger, eventId, exception, message, args);

    /// <summary>Logs a warning-level message with an event ID and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogWarning(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogWarning(hasLogger.Logger, eventId, message, args);

    /// <summary>Logs a warning-level message with an exception and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogWarning(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogWarning(hasLogger.Logger, exception, message, args);

    /// <summary>Logs a warning-level formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogWarning(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogWarning(hasLogger.Logger, message, args);

    //------------------------------------------ERROR------------------------------------------//

    /// <summary>Logs an error-level message with an event ID, exception, and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogError(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogError(hasLogger.Logger, eventId, exception, message, args);

    /// <summary>Logs an error-level message with an event ID and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogError(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogError(hasLogger.Logger, eventId, message, args);

    /// <summary>Logs an error-level message with an exception and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogError(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogError(hasLogger.Logger, exception, message, args);

    /// <summary>Logs an error-level formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogError(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogError(hasLogger.Logger, message, args);

    //------------------------------------------CRITICAL------------------------------------------//

    /// <summary>Logs a critical-level message with an event ID, exception, and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogCritical(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogCritical(hasLogger.Logger, eventId, exception, message, args);

    /// <summary>Logs a critical-level message with an event ID and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogCritical(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogCritical(hasLogger.Logger, eventId, message, args);

    /// <summary>Logs a critical-level message with an exception and formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogCritical(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogCritical(hasLogger.Logger, exception, message, args);

    /// <summary>Logs a critical-level formatted message via <see cref="IHasLogger.Logger"/>.</summary>
    public static void LogCritical(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogCritical(hasLogger.Logger, message, args);

    //------------------------------------------GENERIC LOG------------------------------------------//

    /// <summary>Logs a message at the specified <paramref name="logLevel"/> via <see cref="IHasLogger.Logger"/>.</summary>
    public static void Log(this IHasLogger hasLogger, LogLevel logLevel, string? message, params object?[] args) =>
        LoggerExtensions.Log(hasLogger.Logger, logLevel, message, args);

    /// <summary>Logs a message at the specified <paramref name="logLevel"/> with an event ID via <see cref="IHasLogger.Logger"/>.</summary>
    public static void Log(this IHasLogger hasLogger, LogLevel logLevel, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.Log(hasLogger.Logger, logLevel, eventId, message, args);

    /// <summary>Logs a message at the specified <paramref name="logLevel"/> with an exception via <see cref="IHasLogger.Logger"/>.</summary>
    public static void Log(this IHasLogger hasLogger, LogLevel logLevel, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.Log(hasLogger.Logger, logLevel, exception, message, args);

    /// <summary>Logs a message at the specified <paramref name="logLevel"/> with an event ID and exception via <see cref="IHasLogger.Logger"/>.</summary>
    public static void Log(this IHasLogger hasLogger, LogLevel logLevel, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.Log(hasLogger.Logger, logLevel, eventId, exception, message, args);

    //------------------------------------------SCOPE------------------------------------------//

    /// <summary>Begins a logical operation scope via <see cref="IHasLogger.Logger"/>.</summary>
    public static IDisposable? BeginScope(this IHasLogger hasLogger, string messageFormat, params object?[] args) =>
        LoggerExtensions.BeginScope(hasLogger.Logger, messageFormat, args);
}
