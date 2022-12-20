using System.Reflection.Metadata;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class List
{
    public class Query : IRequest<List<Activity>>{}
    
    public class Handler : IRequestHandler<Query, List<Activity>> 
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Activities.ToListAsync(cancellationToken: cancellationToken);        
        }
    }
}