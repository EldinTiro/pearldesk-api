﻿using ErrorOr;
using MediatR;
using DentFlow.Patients.Domain;

namespace DentFlow.Patients.Application.Commands;

public record UpdatePatientCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string? PreferredName,
    string? ParentName,
    DateOnly? DateOfBirth,
    Gender? Gender,
    string? Pronouns,
    string? Email,
    string? PhoneMobile,
    string? PhoneHome,
    string? PhoneWork,
    ContactMethod? PreferredContactMethod,
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? StateProvince,
    string? PostalCode,
    string? CountryCode,
    string? Occupation,
    Guid? PreferredProviderId,
    bool SmsOptIn,
    bool EmailOptIn,
    string? Notes) : IRequest<ErrorOr<PatientResponse>>;

