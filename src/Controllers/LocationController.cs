using Debug;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Definitions;
using Requests;
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

		if (request.LocationId != locationModel.CurrentLocation?.Id)
		{
			DebugLog.Error("Attempting to use door that is not in the current location");
			return;
		}

		using var db = _ctx.DataFactory.GetCampaignData();

		var newLocation = db.Locations
			.Include(l => l.Doors)
			.SingleOrDefault(l => l.Id == request.DestinationId);

		if (newLocation == null)
		{
			DebugLog.Error("Attempting to use door that leads to non-existent location");
			return;
		}

		_ctx.ModelUpdater.UpdateModel<LocationModel>(
			l => l.CurrentLocation = newLocation
		);
	}

	#region SAVE_AND_LOAD
	private void OnLocationDataLoaded(LocationSaveData saveData)
	{
		using var db = _ctx.DataFactory.GetCampaignData();

		var location = db.Locations
			.Include(l => l.Doors)
			.SingleOrDefault(l => l.Id == saveData.CurrentLocationId)
			;
		if (location == null)
		{
			DebugLog.Error("Could not load current location - ID is invalid");
			return;
		}

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