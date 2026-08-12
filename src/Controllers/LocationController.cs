using Models;
using Models.Definitions;
using Requests;
using Resources.Definitions.Entities;
using Save;

namespace Controllers;

public class LocationController
{
	private readonly ControllerContext _ctx;

	public LocationController(ControllerContext context)
	{
		_ctx = context;
		_ctx.RequestListener.RegisterHandler<OpenDoorRequest>(HandleOpenDoorRequest);

		_ctx.SaveSystem.RegisterLoadHandler<LocationSaveData>("location", OnLocationDataLoaded);
		_ctx.SaveSystem.RegisterSaveHandler("location", GetLocationSaveData);
	}

	private void HandleOpenDoorRequest(OpenDoorRequest request)
	{
		var locationModel = _ctx.ModelGetter.GetModel<ILocationModel>();
		if (locationModel == null) return;

		if (request.LocationId != locationModel.CurrentLocation?.Id) return;

		var newLocation = _ctx.EntityLoader.LoadEntity<Location>(request.DestinationId);
		if (newLocation == null) return;

		_ctx.ModelUpdater.UpdateModel<LocationModel>(
			l => l.CurrentLocation = newLocation
		);
	}

	#region SAVE_AND_LOAD
	private void OnLocationDataLoaded(LocationSaveData saveData)
	{
		var location = _ctx.EntityLoader.LoadEntity<Location>(saveData.CurrentLocationId);
		if (location == null) return;

		_ctx.ModelUpdater.UpdateModel<LocationModel>(
			m => m.CurrentLocation = location
		);
	}

	private LocationSaveData? GetLocationSaveData()
	{
		var locationModel = _ctx.ModelGetter.GetModel<ILocationModel>();
		if (locationModel == null) return null;

		return new LocationSaveData() { CurrentLocationId = locationModel.CurrentLocation?.Id ?? "" };
	}
	#endregion
}