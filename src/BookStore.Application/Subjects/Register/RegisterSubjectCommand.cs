

using BookStore.Application.Messaging;
using BookStore.Application.Subjects.Common;

namespace BookStore.Application.Subjects.Register;

public sealed record RegisterSubjectCommand(string Description): ICommand<SubjectResponse>;