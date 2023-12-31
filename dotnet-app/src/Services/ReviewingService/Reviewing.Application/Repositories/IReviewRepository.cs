﻿using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Application.Repositories;

public interface IReviewRepository
    : IRepository<Review>
{
    Task Add(Review review);
    Task<Review> GetById(ReviewId reviewId);
    Task Update(Review review);
    Task<SubjectGroups?> GetSubjectGroup(string name);
    Task<Subject?> GetSubject(SubjectId subjectId);
}