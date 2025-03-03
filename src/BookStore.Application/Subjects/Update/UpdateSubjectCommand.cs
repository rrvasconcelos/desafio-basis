using BookStore.Application.Messaging;
using BookStore.Application.Subjects.Common;

namespace BookStore.Application.Subjects.Update;

public sealed record UpdateSubjectCommand(int Id, string Description): ICommand<SubjectResponse>;