﻿using GoalManager.UseCases.Organisation.List;
using GoalManager.Web.Common;
using GoalManager.Web.ViewModels.Organisation;
using Microsoft.AspNetCore.Authorization;

namespace GoalManager.Web.Pages.Organisation;

[Authorize]
public class ListModel(IMediator mediator) : PageModelBase
{
  public IList<OrganisationListItemViewModel> Organisations { get; private set; } = new List<OrganisationListItemViewModel>();

  public async Task OnGetAsync(CancellationToken cancellationToken = default)
  {
    var result = await mediator.Send(new ListOrganisationsQuery(null, null), cancellationToken).ConfigureAwait(false);

    AddResultMessages(result);

    if (result.IsSuccess)
    {
      Organisations = result.Value.Select(x => new OrganisationListItemViewModel(x.Id, x.Name, x.TeamsCount)).ToList();
    }
  }
}