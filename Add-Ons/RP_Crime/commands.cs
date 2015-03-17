// --------------------------
// RP Crime :: Commands
// --------------------------

if (isPackage(RP_Crime_Commands))
	deactivatePackage(RP_Crime_Commands);

package RP_Crime_Commands
{
	function ServerCmdGiveMoney(%client, %arg0)
	{
		if (!%client.isInRP())
			return;
		%money1 = %client.RPData.value["money"];
		%target = Parent::ServerCmdGiveMoney(%client, %arg0);
		%money2 = %client.RPData.value["money"];
		%stolen = %client.RPData.value["stolenMoney"];
		if (%stolen == 0 || %money1 == %money2 || (!isObject(%target)))
			return;
		
		%client.TransferDirtyMoney(%target, %money1 - %money2);
	}
};
RPRegPackage(RP_Crime_Commands);

///////////////////////
// Advanced commands //
///////////////////////

// Clear player record
function ServerCmdClearRecord(%client, %n0, %n1, %n2, %n3)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
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
	
	%target.RPData.value["demerits"] = 0;
	%target.RPData.value["jail"] = -1;
	%target.RPData.value["stolenMoney"] = 0;
	RemoveFromCriminalList(%target.getSaveKey());
	RemoveFromJailList(%target.getSaveKey());
	
	messageClient(%client, '', '\c6You cleared \c3%1\c6\'s record.', %target.name);
	messageClient(%target, '', '\c3%1\c6 cleared your record.', %client.name);
	
	%target.displayInfo();
}

//function serverCmdForgive(%client) 
//{
 //   if(!$Last::used)
 //   {
 //       if(%client == $Last::death)
 //       {
 //                       %killer = $Last::killer;
 //                       CityRPGData.getData(%killer.bl_id).valueDemerits -= $CityRPG::demerits::murder;
  //                      $Last::used=true;
 //                       messageClient(%client, '', "You've forgive.");
 //                       messageAll('',"\c3" @ %client.name SPC "\c6 has forgiven\c3" SPC %killer.name @ "\c6.");
 //                       return messageClient(%killer, '', "Someone forgave you.");
 //       }
 //       else
  //          return messageClient(%client, '', "You weren't the last death.");
  //      }
  //      else
  //              return messageClient(%client, '', "Someone already forgave.");
//}
