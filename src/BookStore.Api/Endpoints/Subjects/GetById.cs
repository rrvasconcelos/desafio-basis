using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Subjects.GetById;
using MediatR;

namespace BookStore.Api.Endpoints.Subjects;

public sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("subjects/{id:int}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetSubjectByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Subjects);
    }
}