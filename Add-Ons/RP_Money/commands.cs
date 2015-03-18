// --------------------------
// RP Money :: Commands
// --------------------------

if (isPackage(RP_Money_Commands))
	deactivatePackage(RP_Money_Commands);

package RP_Money_Commands
{
	function ServerCmdMessageSent(%client, %text)
	{
		if (!%client.isInRP())
		{
			Parent::ServerCmdMessageSent(%client, %text);
			return;
		}
		
		// Handle event transfers
		if (isObject(%client.activateBrick) && !isObject(%client.activateDatablock))
		{
			if (RequestTransfer(%client, %client.activateBrick, %client.activateTransfer, mFloor(%client.cost), %text))
				%client.activateBrick.onTransferSuccess(%client);
			%client.activateTransfer = "";
			%client.cost = 0;
			%client.activateBrick = 0;
			%client.displayInfo();
			return;
		}
		
		Parent::ServerCmdMessageSent(%client, %text);
	}
};
RPRegPackage(RP_Money_Commands);

// Give money to player
function ServerCmdGiveMoney(%client, %arg0)
{
	if (!$RP::activated)
		return 0;
	
	%give = atoi(%arg0);
	
	// Invalid command
	if (%give <= 0)
	{
		commandToClient(%client, 'centerPrint', "\c3Invalid amount.", 2);
		return 0;
	}
	
	%player = %client.player;
	// Player exists
	if (isObject(%player))
		%obj = %player.GetObjectFromPOW($TypeMasks::PlayerObjectType);
	
	// Nobody to point at
	if (!(isObject(%obj) && (%obj.getType() & $TypeMasks::PlayerObjectType) && isObject(%obj.client)))
	{
		commandToClient(%client, 'centerPrint', "\c3You have to look at somebody.", 2);
		return 0;
	}
	
	%target = %obj.client;
	// Have enough money
	if (%client.RPData.value["money"] <= 0)
	{
		commandToClient(%client, 'centerPrint', "\c3You have nothing to give.", 2);
		return %target;
	}
	
	// Reduce money to the amount player have on himself
	%give = (%client.RPData.value["money"] >= %give) ? %give : %client.RPData.value["money"];
	
	%client.RPData.value["money"] -= %give;
	%target.RPData.value["money"] += %give;
	messageClient(%client, '', '\c6You gave \c3%1\c6 to \c3%2\c6.', prepareDisplayMoney(%give), %target.name);
	messageClient(%target, '', '\c3%1\c6 gave you \c3%2\c6.', %client.name, prepareDisplayMoney(%give));
	%target.RPData.value["BusinessExp"] += 1;
	return %target;
}

///////////////////////
// Advanced commands //
///////////////////////

// Grant money
function ServerCmdGrantMoney(%client, %amount, %n0, %n1, %n2, %n3)
{
	if (!$RP::activated)
		return;
	//if (!%client.CheckPrefTrust($RP::pref::server::rankAdvanceCommands))
	//	return;
	if(!%client.isSuperAdmin)
		return;
	
	%amount = atoi(%amount);
	if (%amount <= 0)
	{
		messageClient(%client, '', "\c6Invalid amount.");
		return;
	}
	
	%name = trim(%n0 SPC %n1 SPC %n2 SPC %n3);
	%target = (%name $= "") ? %client : findClientByText(%name);
	
	if (!isObject(%target))
	{
		messageClient(%client, '', "\c6Invalid player.");
		return;
	}
	
	%target.RPData.value["money"] += %amount;
	
	messageClient(%client, '', '\c6You granted \c3%1 %2\c6 to \c3%3\c6.', prepareDisplayMoney(getMetricPrefix(%amount)), $RP::pref::moneyName, %target.name);
	messageClient(%target, '', '\c3%1\c6 granted you \c3%2 %3\c6.', %client.name, prepareDisplayMoney(getMetricPrefix(%amount)), $RP::pref::moneyName);
	
	%target.displayInfo();
}

// Deduct money
function ServerCmdDeductMoney(%client, %amount, %n0, %n1, %n2, %n3)
{
	if (!$RP::activated)
		return;
	if (!%client.CheckPrefTrust($RP::pref::server::rankAdvanceCommands))
		return;
	
	%amount = atoi(%amount);
	if (%amount <= 0)
	{
		messageClient(%client, '', "\c6Invalid amount.");
		return;
	}
	
	%name = trim(%n0 SPC %n1 SPC %n2 SPC %n3);
	%target = (%name $= "") ? %client : findClientByText(%name);
	
	if (!isObject(%target))
	{
		messageClient(%client, '', "\c6Invalid player.");
		return;
	}
	
	%target.RPData.value["money"] -= %amount;
	
	messageClient(%client, '', '\c6You deducted \c3%1 %2\c6 from \c3%3\c6.', prepareDisplayMoney(getMetricPrefix(%amount)), $RP::pref::moneyName, %target.name);
	messageClient(%target, '', '\c3%1\c6 deducted \c3%2 %3\c6 from you.', %client.name, prepareDisplayMoney(getMetricPrefix(%amount)), $RP::pref::moneyName);
	
	%target.displayInfo();
}

// Clear out loan
function ServerCmdClearLoan(%client, %n0, %n1, %n2, %n3)
{
	if (!$RP::activated)
		return;
	if (!%client.CheckPrefTrust($RP::pref::server::rankAdvanceCommands))
		return;
	
	%name = trim(%n0 SPC %n1 SPC %n2 SPC %n3);
	%target = (%name $= "") ? %client : findClientByText(%name);
	
	if (!isObject(%target))
	{
		messageClient(%client, '', "\c6Invalid player.");
		return;
	}
	
	%target.RPData.value["loan"] = 0;
	
	messageClient(%client, '', '\c6You cleared \c3%1\c6\'s loan depts.', %target.name);
	messageClient(%target, '', '\c3%1\c6 cleared your loan depts.', %client.name);
	
	%target.displayInfo();
}

/////////
// GUI //
/////////

// GUI
function ServerCmdBank_Deposit(%client, %amount)
{
	if (!$RP::activated)
		return;
	if (!isObject(%client.activateDatablock))
		return;
	
	%amount = atoi(%amount);
	if (%amount <= 0)
	{
		commandToClient(%client, 'messageBoxOK', "RP Error", "Not a valid number.");
		return;
	}
	
	if (%amount > %client.RPData.value["money"])
		%amount = %client.RPData.value["money"];
	
	%client.RPData.value["bank"] += %amount;
	%client.RPData.value["money"] -= %amount;
	%client.activateDatablock = 0;
}
function ServerCmdBank_Withdraw(%client, %amount)
{
	if (!$RP::activated)
		return;
	if (!isObject(%client.activateDatablock))
		return;
	
	%amount = atoi(%amount);
	if (%amount <= 0)
	{
		commandToClient(%client, 'messageBoxOK', "RP Error", "Not a valid number.");
		return;
	}
	
	if (%amount > %client.RPData.value["bank"])
		%amount = %client.RPData.value["bank"];
	
	%client.RPData.value["bank"] -= %amount;
	%client.RPData.value["money"] += %amount;
}
