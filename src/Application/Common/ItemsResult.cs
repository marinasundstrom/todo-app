﻿namespace TodoApp.Application.Common;

public sealed record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);