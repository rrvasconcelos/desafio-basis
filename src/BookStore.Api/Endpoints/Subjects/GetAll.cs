using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Subjects.GetAll;
using MediatR;

namespace BookStore.Api.Endpoints.Subjects;

public sealed class GetAll: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("subjects", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetSubjectAllQuery();

                var result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Subjects);
        
    }
}