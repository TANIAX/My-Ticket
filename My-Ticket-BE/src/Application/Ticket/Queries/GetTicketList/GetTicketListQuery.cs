using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Ticket.Queries.GetTicketList
{
    public class GetTicketListQuery : IRequest<List<TicketListDTO>>
    {
        public int CreationDate { get; set; }
        public string Requester { get; set; }
        public string AssignTO { get; set; }
        public int Group { get; set; }
        public int Priority { get; set; }
        public int Type { get; set; }
        public int Project { get; set; }
        public int Status { get; set; }

        public class GroupListQueryHandler : TicketHeader, IRequestHandler<GetTicketListQuery, List<TicketListDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _userManager;
            public GroupListQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IIdentityService userManager)
            {
                _context = context;
                _mapper = mapper;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<List<TicketListDTO>> Handle(GetTicketListQuery request, CancellationToken cancellationToken)
            {
                bool currentUserIsMemberOrAdmin = false;
                List<TicketListDTO> vm = new List<TicketListDTO>();

                if (await _userManager.IsInRoleAsync(_currentUserService.UserId, "Admin") || await _userManager.IsInRoleAsync(_currentUserService.UserId, "Member"))
                    currentUserIsMemberOrAdmin = true;

                vm = await _context.TickerHeader
                     .ProjectTo<TicketListDTO>(_mapper.ConfigurationProvider)
                     .OrderBy(t => t.CreationDate)
                     .ToListAsync(cancellationToken);
                if (currentUserIsMemberOrAdmin)
                {
                    if (request.Requester != "")
                    {
                        vm = vm.Where(x => x.Requester?.Id == request.Requester).ToList();
                    }
                    if (request.AssignTO != "")
                    {
                        vm = vm.Where(x => x.AssignTO?.Id == request.AssignTO).ToList();
                    }
                    if (request.Group != 0)
                    {
                        vm = vm.Where(x => x.Group?.Id == request.Group).ToList();
                    }
                    if (request.Status != 0)
                    {
                        if (request.Status == -1)
                        {
                            vm = vm.Where(x => x.Status?.Name != "Closed").ToList();
                        }
                        else
                        {
                            vm = vm.Where(x => x.Status?.Id == request.Status).ToList();
                        }

                    }
                    if (request.Type != 0)
                    {
                        vm = vm.Where(x => x.Type?.Id == request.Type).ToList();
                    }
                    if (request.Priority != 0)
                    {
                        vm = vm.Where(x => x.Priority?.Id == request.Priority).ToList();
                    }
                    if (request.Project != 0)
                    {
                        vm = vm.Where(x => x.Project?.Id == request.Project).ToList();
                    }

                    if (request.CreationDate != 0)
                    {
                        DateTime now = DateTime.Now;
                        DateTime startDate;
                        DateTime endDate;
                        switch (request.CreationDate)
                        {
                            case -1:
                                startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                                endDate = startDate.AddDays(1);
                                vm = vm.Where(x => x.CreationDate > startDate && x.CreationDate < endDate).ToList();
                                break;
                            case 1:
                                startDate = new DateTime(now.Year, now.Month, now.Day - 1, 0, 0, 0);
                                endDate = startDate.AddDays(1);
                                vm = vm.Where(x => x.CreationDate > startDate && x.CreationDate < endDate).ToList();
                                break;
                            case 7:
                                DateTime startOfWeek = DateTime.Today;
                                int delta = DayOfWeek.Monday - startOfWeek.DayOfWeek;
                                startOfWeek = startOfWeek.AddDays(delta);
                                var endOfWeek = startOfWeek.AddDays(7);

                                vm = vm.Where(x => x.CreationDate > startOfWeek && x.CreationDate < endOfWeek).ToList();
                                break;
                            case 30:
                                vm = vm.Where(x => x.CreationDate.Month == now.Month && x.CreationDate.Year == now.Year).ToList();
                                break;
                            default:
                                break;
                        }
                    }
                    if (vm != null)
                    {
                        foreach (var v in vm)
                        {
                            TicketListUserDTO user;
                            List<TicketLine> tl = new List<TicketLine>();
                            string u = "";
                            try
                            {
                                tl = _context.TickerHeader.Include("TicketLine")
                                .Where(x => x.Id == v.Id)
                                .FirstOrDefault().TicketLine.ToList();

                                if (tl.Count > 0)
                                {
                                    u = tl.LastOrDefault().ResponseBy?.Id;
                                    if (u != "")
                                    {
                                        user = await _context.User.ProjectTo<TicketListUserDTO>(_mapper.ConfigurationProvider)
                                                             .Where(x => x.Id == u)
                                                             .FirstOrDefaultAsync(cancellationToken);

                                        v.LastResponseByUser = user;
                                    }
                                    else
                                    {
                                        v.LastResponseByEmail = u;
                                    }
                                    v.LastResponseDate = tl.LastOrDefault().Created;
                                }
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        vm = new List<TicketListDTO>();
                    }
                }
                //If user is a customer he cannot filter, he can just see his own list of ticket.
                else
                {
                    vm = vm.Where(x => x.Requester?.Id == _currentUserService.UserId).OrderBy(x => x.CreationDate).ToList();
                }


                return vm;
            }
        }
    }
}

