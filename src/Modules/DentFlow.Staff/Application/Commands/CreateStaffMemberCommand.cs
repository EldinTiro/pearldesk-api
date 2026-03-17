﻿using ErrorOr;
using MediatR;
using DentFlow.Staff.Domain;

namespace DentFlow.Staff.Application.Commands;

public record CreateStaffMemberCommand(
    StaffType StaffType,
    string FirstName,
    string LastName,
    string? Email,
    string? Phone,
    DateOnly? HireDate,
    string? Specialty,
    string? ColorHex) : IRequest<ErrorOr<StaffMemberResponse>>;

