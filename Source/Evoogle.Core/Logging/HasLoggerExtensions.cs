// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

namespace Evoogle.Logging;

[SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "Forwarding wrapper to Microsoft.Extensions.Logging.LoggerExtensions.")]
[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "Forwarding wrapper; delegates not applicable for arbitrary templates.")]
public static class HasLoggerExtensions
{
    //------------------------------------------DEBUG------------------------------------------//

    public static void LogDebug(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogDebug(hasLogger.Logger, eventId, exception, message, args);

    public static void LogDebug(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogDebug(hasLogger.Logger, eventId, message, args);

    public static void LogDebug(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogDebug(hasLogger.Logger, exception, message, args);

    public static void LogDebug(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogDebug(hasLogger.Logger, message, args);

    //------------------------------------------TRACE------------------------------------------//

    public static void LogTrace(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogTrace(hasLogger.Logger, eventId, exception, message, args);

    public static void LogTrace(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogTrace(hasLogger.Logger, eventId, message, args);

    public static void LogTrace(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogTrace(hasLogger.Logger, exception, message, args);

    public static void LogTrace(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogTrace(hasLogger.Logger, message, args);

    //------------------------------------------INFORMATION------------------------------------------//

    public static void LogInformation(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogInformation(hasLogger.Logger, eventId, exception, message, args);

    public static void LogInformation(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogInformation(hasLogger.Logger, eventId, message, args);

    public static void LogInformation(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogInformation(hasLogger.Logger, exception, message, args);

    public static void LogInformation(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogInformation(hasLogger.Logger, message, args);

    //------------------------------------------WARNING------------------------------------------//

    public static void LogWarning(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogWarning(hasLogger.Logger, eventId, exception, message, args);

    public static void LogWarning(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogWarning(hasLogger.Logger, eventId, message, args);

    public static void LogWarning(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogWarning(hasLogger.Logger, exception, message, args);

    public static void LogWarning(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogWarning(hasLogger.Logger, message, args);

    //------------------------------------------ERROR------------------------------------------//

    public static void LogError(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogError(hasLogger.Logger, eventId, exception, message, args);

    public static void LogError(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogError(hasLogger.Logger, eventId, message, args);

    public static void LogError(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogError(hasLogger.Logger, exception, message, args);

    public static void LogError(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogError(hasLogger.Logger, message, args);

    //------------------------------------------CRITICAL------------------------------------------//

    public static void LogCritical(this IHasLogger hasLogger, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogCritical(hasLogger.Logger, eventId, exception, message, args);

    public static void LogCritical(this IHasLogger hasLogger, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.LogCritical(hasLogger.Logger, eventId, message, args);

    public static void LogCritical(this IHasLogger hasLogger, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.LogCritical(hasLogger.Logger, exception, message, args);

    public static void LogCritical(this IHasLogger hasLogger, string? message, params object?[] args) =>
        LoggerExtensions.LogCritical(hasLogger.Logger, message, args);

    //------------------------------------------GENERIC LOG------------------------------------------//

    public static void Log(this IHasLogger hasLogger, LogLevel logLevel, string? message, params object?[] args) =>
        LoggerExtensions.Log(hasLogger.Logger, logLevel, message, args);

    public static void Log(this IHasLogger hasLogger, LogLevel logLevel, EventId eventId, string? message, params object?[] args) =>
        LoggerExtensions.Log(hasLogger.Logger, logLevel, eventId, message, args);

    public static void Log(this IHasLogger hasLogger, LogLevel logLevel, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.Log(hasLogger.Logger, logLevel, exception, message, args);

    public static void Log(this IHasLogger hasLogger, LogLevel logLevel, EventId eventId, Exception? exception, string? message, params object?[] args) =>
        LoggerExtensions.Log(hasLogger.Logger, logLevel, eventId, exception, message, args);

    //------------------------------------------SCOPE------------------------------------------//

    public static IDisposable? BeginScope(this IHasLogger hasLogger, string messageFormat, params object?[] args) =>
        LoggerExtensions.BeginScope(hasLogger.Logger, messageFormat, args);
}
