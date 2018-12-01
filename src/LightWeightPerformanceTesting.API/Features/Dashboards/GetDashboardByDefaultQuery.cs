using LightWeightPerformanceTesting.API.Features.Cards;
using LightWeightPerformanceTesting.API.Features.DashboardCards;
using LightWeightPerformanceTesting.Core.Common;
using LightWeightPerformanceTesting.Core.Interfaces;
using LightWeightPerformanceTesting.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LightWeightPerformanceTesting.API.Features.Dashboards
{
    public class GetDashboardByDefaultQuery
    {
        public class Request : AuthenticatedRequest<Response>, IRequest<Response> { }

        public class Response
        {
            public DashboardDto  Dashboard { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {

            private readonly IRepository _repository;

            public Handler(IRepository repository) => _repository = repository;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var dashboards = _repository.Query<Dashboard>().ToList();

                var dashboard = _repository.Query<Dashboard>()
                    .Single(x => x.Name == "Default");

                var dashboardDto = DashboardDto.FromDashboard(dashboard);

                var dashboardCardDtos = new List<DashboardCardDto>();

                foreach(var dashboardCardId in dashboard.DashboardCardIds)
                {
                    var dashboardCardDto = DashboardCardDto.FromDashboardCard(_repository.Query<DashboardCard>(dashboardCardId));
                    dashboardCardDto.Card = CardDto.FromCard(_repository.Query<Card>(dashboardCardDto.CardId));                    
                    dashboardCardDtos.Add(dashboardCardDto);
                }
                       
                return Task.FromResult(new Response()
                {
                    Dashboard = DashboardDto.FromDashboard(dashboard, dashboardCardDtos)
                });
            }
        }
    }
}
