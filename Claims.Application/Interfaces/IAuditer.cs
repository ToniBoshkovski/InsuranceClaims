﻿namespace Claims.Application.Interfaces;

public interface IAuditer
{
    Task AuditClaim(string id, string httpRequestType);

    Task AuditCover(string id, string httpRequestType);
}