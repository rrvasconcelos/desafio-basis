using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Authors.Register;
using BookStore.Application.Subjects.Register;
using MediatR;

namespace BookStore.Api.Endpoints.Subjects;

public class Register: IEndpoint
{
    private sealed record Request(string Description);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("subjects", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new RegisterSubjectCommand(request.Description);

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Subjects);
    }
}