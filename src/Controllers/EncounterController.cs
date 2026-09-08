using Data;
using Data.Definitions.Triggers;
using Debug;
using Events.Definitions;
using Events.Definitions.Game;
using Microsoft.EntityFrameworkCore;
using View.Definitions;
using View.Views;

namespace Controllers;

public class EncounterController(
	IEventHandler eventHandler,
	IViewManager viewManager,
	SessionFactory sessionFactory)
{
	private readonly SessionFactory _sessionFactory = sessionFactory;
	private readonly IViewManager _viewManager = viewManager;
	private readonly IEventHandler _eventHandler = eventHandler;

	private readonly Queue<int> _pendingEncounters = [];

	public void InitializeTriggers()
	{
		using var db = _sessionFactory.GetReadonlySession();

		foreach (var encounter in db.Encounters
			.Include(e => e.Trigger)
			.Where(e => !e.IsComplete || e.IsRepeatable)
			.AsEnumerable())
		{
			if (encounter.Trigger == null)
			{
				DebugLog.Error($"Encounter with Id {encounter.Id} has no trigger");
				continue;
			}

			RegisterTrigger(encounter.Trigger);
		}
	}

	private void RegisterTrigger(Trigger trigger)
	{
		if (trigger is CharacterInteractionTrigger characterInteractionTrigger)
		{
			_eventHandler.Register<CharacterInteractionEvent>(@event =>
			{
				if (@event.CharacterId == characterInteractionTrigger.CharacterId)
				{
					TriggerEncounter(trigger.Id);
				}
			});
		}
	}

	private void TriggerEncounter(int triggerId)
	{
		using var db = _sessionFactory.GetReadonlySession();

		var encounter = db.Encounters.FirstOrDefault(e => e.Trigger.Id == triggerId);
		if (encounter == null)
		{
			DebugLog.Error($"Trying to trigger encounter, but none exist with trigger Id {triggerId}");
			return;
		}

		if (encounter.IsComplete && !encounter.IsRepeatable)
		{
			return;
		}

		var step = encounter.IsComplete ? encounter.RepeatStep : 1;
		_viewManager.ShowView(new EncounterView(encounter.Id, step));
	}
}