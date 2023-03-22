using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecSysApi.Domain.Models;

namespace RecSysApi.Domain.Interfaces.Repositories;

public interface IFeedbackRepository : IRepository<Feedback>
{
    Task<List<Feedback>> GetFeedbacksByQueryId(Guid queryId);
    Task<List<Feedback>> GetFeedbacksByVideoId(Guid videoId);
}