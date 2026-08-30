using Debug;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Definitions;
using Requests;
using Save;

namespace Controllers;

public class LocationController : AController
{
	public LocationController(ControllerContext ctx) : base(ctx)
	{
		_ctx.RequestListener.RegisterHandler<OpenDoorRequest>(HandleOpenDoorRequest);
		_ctx.SaveSystem.RegisterLoadHandler<LocationSaveData>("location", OnLocationDataLoaded);
		_ctx.SaveSystem.RegisterSaveHandler("location", GetLocationSaveData);
	}

	private void HandleOpenDoorRequest(OpenDoorRequest request)
	{

		var locationModel = _ctx.ModelGetter.GetModel<ILocationModel>();
		if (locationModel == null) return;

		if (request.Door.FromId != locationModel.CurrentLocation?.Id)
		{
			DebugLog.Error("Attempting to use door that is not in the current location");
			return;
		}

		_ctx.ModelUpdater.UpdateModel<LocationModel>(
			l => l.CurrentLocation = request.Door.To
		);
	}

	#region SAVE_AND_LOAD
	private void OnLocationDataLoaded(LocationSaveData saveData)
	{
		using var db = _ctx.DataFactory.GetCampaignData();

		var location = db.Locations
			.Include(l => l.ItemPickups)
			.ThenInclude(p => p.Item)
			.Include(l => l.DoorsOut)
			.ThenInclude(d => d.To)
			.SingleOrDefault(l => l.Id == saveData.CurrentLocationId);

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
		return locationModel == null ?
			null :
			new LocationSaveData()
			{
				CurrentLocationId = locationModel.CurrentLocation?.Id ?? 1
			};
	}
	#endregion
}