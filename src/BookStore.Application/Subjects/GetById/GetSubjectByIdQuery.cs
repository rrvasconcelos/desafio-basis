using BookStore.Application.Messaging;
using BookStore.Application.Subjects.Common;

namespace BookStore.Application.Subjects.GetById;

public sealed record GetSubjectByIdQuery(int Id) : IQuery<SubjectResponse>;