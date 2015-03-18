// ============================================================
// Section 4 : Trigger Bullshit
// ============================================================
function CRPGLotTriggerData::onEnterTrigger(%this, %trigger, %obj)
{
	parent::onEnterTrigger(%this, %trigger, %obj);
	
	if(!isObject(%obj.client))
	{
		if(isObject(%obj.getControllingClient()))
			%client = %obj.getControllingClient();
		else
			return;
	}
	else
		%client = %obj.client;
	
	%trigger.parent.onEnterLot(%obj);
	
	%client.CRPGTrigger = %trigger;
	%client.CRPGLotBrick = %trigger.parent;
	
	%client.displayInfo();
}

function CRPGLotTriggerData::onLeaveTrigger(%this, %trigger, %obj)
{
	if(!isObject(%obj.client))
	{
		if(isObject(%obj.getControllingClient()))
			%client = %obj.getControllingClient();
		else
			return;
	}
	else
		%client = %obj.client;
	
	%trigger.parent.onLeaveLot(%obj);
	
	%client.CRPGTrigger = "";
	%client.CRPGLotBrick = "";
	
	%client.displayInfo();
}

function CRPGInputTriggerData::onEnterTrigger(%this, %trigger, %obj)
{
	if(!isObject(%obj.client))
	{
		return;
	}
	
	%obj.client.CRPGTrigger = %trigger;
	
	%trigger.parent.getDatablock().parseData(%trigger.parent, %obj.client, true, "");
}

function CRPGInputTriggerData::onLeaveTrigger(%this, %trigger, %obj, %a)
{
	if(!isObject(%obj.client))
	{
		return;
	}
	
	if(%obj.client.CRPGTrigger == %trigger)
	{
		%trigger.parent.getDatablock().parseData(%trigger.parent, %obj.client, false, "");
		
		%obj.client.CRPGTrigger = "";
	}
}
