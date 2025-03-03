using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Subjects.Update;
using MediatR;

namespace BookStore.Api.Endpoints.Subjects;

public sealed class Update: IEndpoint
{
    private sealed record Request(int Id, string Description);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("subjects/{id:int}", async (int id, Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("Id does not match");
                }
                
                var command = new UpdateSubjectCommand(request.Id, request.Description);

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Subjects);
    }
}