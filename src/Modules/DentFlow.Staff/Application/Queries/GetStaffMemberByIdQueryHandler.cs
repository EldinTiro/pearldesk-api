using ErrorOr;
using MediatR;
using DentFlow.Staff.Application.Interfaces;
using DentFlow.Staff.Domain;

namespace DentFlow.Staff.Application.Queries;

public class GetStaffMemberByIdQueryHandler(IStaffRepository staffRepository)
    : IRequestHandler<GetStaffMemberByIdQuery, ErrorOr<StaffMemberResponse>>
{
    public async Task<ErrorOr<StaffMemberResponse>> Handle(
        GetStaffMemberByIdQuery query,
        CancellationToken cancellationToken)
    {
        var staffMember = await staffRepository.GetByIdAsync(query.Id, cancellationToken);
        if (staffMember is null)
            return StaffErrors.NotFound;

        return StaffMemberResponse.FromEntity(staffMember);
    }
}

