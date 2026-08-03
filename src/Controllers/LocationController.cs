using Models;
using Models.Definitions;
using Requests;
using Requests.Definitions;
using Resources.Definitions;
using Resources.Definitions.Entities;

namespace Controllers;

public class LocationController
{
	private readonly ModelRegister _modelRegister;
	private readonly IEntityLoader _entityLoader;

	public LocationController(
		IRequestRegister requestRegister,
		ModelRegister modelRegister,
		IEntityLoader entityLoader
	)
	{
		_modelRegister = modelRegister;
		_entityLoader = entityLoader;

		requestRegister.RegisterHandler<OpenDoorRequest>(HandleOpenDoorRequest);
	}

	private void HandleOpenDoorRequest(OpenDoorRequest request)
	{
		var locationModel = _modelRegister.GetModel<ILocationModel>();
		if (locationModel == null) return;

		if (request.LocationId != locationModel.CurrentLocation?.Id) return;

		var newLocation = _entityLoader.LoadEntity<Location>(request.DestinationId);
		if (newLocation == null) return;

		_modelRegister.UpdateModel<LocationModel>(
			l => l.CurrentLocation = newLocation
		);
	}
}