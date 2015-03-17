// ============================================================
// Section 4 : ClothesSO
// ============================================================
function ClothesSO::loadClothes(%so)
{
	// Clothing Data
	%so.color["none"]		= "1 1 1 1";
	%so.node["none"]		= "0";
	
	// Outfits
	// Outfits use index instead of names.
	// Do not repeat indexes.
	// This is the order they appear in the GUI.
	%so.str[1]	= "none none none none whitet whitet skin bluejeans blackshoes default default";
	%so.uiName[1]	= "Default";
	%so.sellName[1]	= "Default Suit";

	%so.str[2]	= "none brownhat keep keep greenshirt greenshirt keep greyPants blackshoes default default";
	%so.uiName[2]	= "Basic";
	%so.sellName[2]	= "Basic Outfit";
	
	%so.str[3]	= "keep skullcap keep keep blackshirt blackshirt blackgloves blackPants blackshoes default default";
	%so.uiName[3]	= "Gimp";
	%so.sellName[3]	= "Gimp Suit";
	
	%so.str[4]	= "none none none none whitet redsleeve keep brightbluePants blueshoes default default";
	%so.uiName[4]	= "Blockhead";
	%so.sellName[4]	= "Blockhead Clothes";

	%so.str[5]	= "keep keep keep keep greenshirt greenshirt keep brownPants blackshoes default worm-sweater";
	%so.uiName[5]	= "Nerd";
	%so.sellName[5]	= "Nerd Suit";

	%so.str[6]	= "keep keep keep keep blackshirt blackshirt keep blackPants blackshoes default Mod-Suit";
	%so.uiName[6]	= "Business";
	%so.sellName[6]	= "Business Suit";

	%so.str[7]	= "keep keep keep keep blueshirt blueshirt keep bluePants blackshoes default Mod-Suit";
	%so.uiName[7]	= "Council";
	%so.sellName[7]	= "Council Suit";
	
	%so.str[8]	= "keep keep keep keep blackshirt blackshirt skingen blackpants blackshoes default Mod-Suit";
	%so.uiName[8]	= "Suit";
	%so.sellName[8]	= "Suit & Tie";
	
	%so.str[9]	= "DrKleiner DrKleiner DrKleiner DrKleiner whitet whitet brightbluegloves whitet blackshoes DrKleiner DrKleiner";
	%so.uiName[9]	= "Doctor";
	%so.sellName[9]	= "Doctor";
	
	// Hats
	%so.color["brownhat"]	= "0.329 0.196 0.000 1.000";
	%so.node["brownhat"]	= "4";
	%so.str["brownhat"]	= "keep this keep keep keep keep keep keep keep";
	
	%so.color["piratehat"]	= "0.078 0.078 0.078 1";
	%so.node["piratehat"]	= "5";
	%so.str["piratehat"]	= "keep this keep keep keep keep keep keep keep";
	
	%so.color["copHat"]	= "0 0.141176 0.333333 1";
	%so.node["copHat"]	= "6";
	%so.str["copHat"]	= "keep this keep keep keep keep keep keep keep";
	
    %so.color["redCopHat"]	= "1.000 0.000 0.000 1.000";
	%so.node["redCopHat"]	= "6";
	%so.str["redCopHat"]	= "keep this keep keep keep keep keep keep keep";
	
	%so.color["skullcap"]	= "0.200 0.200 0.200 1.000";
	%so.node["skullcap"]	= "7";
	%so.str["skullcap"]	= "keep this keep keep keep keep keep keep keep";
	
	// Gloves
	%so.color["blackgloves"] = "0.200 0.200 0.200 1.000";
	%so.node["blackgloves"]	= "0";
	%so.str["blackgloves"]	= "keep keep keep keep keep keep this keep keep";
	
	%so.color["brightbluegloves"] = "0.500 0.400 0.800 1.000";
	%so.node["brightbluegloves"]	= "0";
	%so.str["brightbluegloves"]	= "keep keep keep keep keep keep this keep keep";
	
	// Shirts
	%so.color["pinkt"]	= "1 0.75 0.79 1";
	%so.node["pinkt"]	= "gender";
	%so.str["pinkt"]	= "keep keep keep keep this this keep keep keep";
	
	%so.color["pinktank"]	= "1 0.07 0.57 1";
	%so.node["pinktank"]	= "gender";
	%so.str["pinktank"]	= "keep keep keep keep this skingen keep keep keep";
	
	%so.color["whitet"]	= "1 1 1 1";
	%so.node["whitet"]	= "gender";
	%so.str["whitet"]	= "keep keep keep keep this this keep keep keep";
	
	%so.color["copShirt"]	= "0 0.141176 0.333333 1";
	%so.node["copShirt"]	= "gender";
	%so.str["copShirt"]	= "keep keep keep keep this this keep keep keep";
	
    %so.color["redCopShirt"]	= "1.000 0.000 0.000 1.000";
	%so.node["redCopShirt"]	= "gender";
	%so.str["redCopShirt"]	= "keep keep keep keep this this keep keep keep";
	
	%so.color["jumpsuit"]	= "1 0.617 0 1";
	%so.node["jumpsuit"]	= "gender";
	%so.str["jumpsuit"]	= "keep keep keep keep this this keep this this";
	
	%so.color["blackshirt"]	= "0.200 0.200 0.200 1.000";
	%so.node["blackshirt"]	= "gender";
	%so.str["blackshirt"]	= "keep keep keep keep this this keep keep keep";
	
	%so.color["brownshirt"]	= "0.329 0.196 0.000 1.000";
	%so.node["brownshirt"]	= "gender";
	%so.str["brownshirt"]	= "keep keep keep keep this this keep keep keep";
	
	%so.color["greenshirt"]	= "0.00 0.262 0.00 1";
	%so.node["greenshirt"]	= "gender";
	%so.str["greenshirt"]	= "keep keep keep keep this this keep keep keep";

	%so.color["blueshirt"]	= "0.0 0.141 0.333 1";
	%so.node["blueshirt"]	= "gender";
	%so.str["blueshirt"]	= "keep keep keep keep this this keep keep keep";

	// Pants
	%so.color["bluejeans"]	= "0 0.141 0.333 1";
	%so.node["bluejeans"]	= "0";
	%so.str["bluejeans"]	= "keep keep keep keep keep keep keep this keep";
	
	%so.color["blackPants"] = "0.200 0.200 0.200 1.000";
	%so.node["blackPants"]	= "0";
	%so.str["blackPants"]	= "keep keep keep keep keep keep keep this keep";
	
	%so.color["brownPants"] = "0.329 0.196 0.000 1.000";
	%so.node["brownPants"]	= "0";
	%so.str["brownPants"]	= "keep keep keep keep keep keep keep this keep";
	
	%so.color["greyPants"] = "0.300 0.300 0.300 1.000";
	%so.node["greyPants"]	= "0";
	%so.str["greyPants"]	= "keep keep keep keep keep keep keep this keep";

	%so.color["brightbluePants"] = "0.200 0.000 0.800 1.000";
	%so.node["brightbluePants"]	= "0";
	%so.str["brightbluePants"]	= "keep keep keep keep keep keep keep this keep";

	%so.color["bluePants"] = "0.0 0.141 0.333 1";
	%so.node["bluePants"]	= "0";
	%so.str["bluePants"]	= "keep keep keep keep keep keep keep this keep";

	// Shoes
	%so.color["blackshoes"]	= "0.200 0.200 0.200 1.000";
	%so.node["blackshoes"]	= "0";
	%so.str["blackshoes"]	= "keep keep keep keep keep keep keep keep this";

	%so.color["brownshoes"]	= "0.329 0.196 0.000 1.000";
	%so.node["brownshoes"]	= "0";
	%so.str["brownshoes"]	= "keep keep keep keep keep keep keep keep this";

	%so.color["blueshoes"]	= "0.200 0.000 0.800 1.000";
	%so.node["blueshoes"]	= "0";
	%so.str["bluehoes"]	= "keep keep keep keep keep keep keep keep this";

	//Misc
	%so.color["redsleeve"] = "1.000 0.000 0.000 1.000";
	%so.node["redsleeve"] = "gender";
	%so.str["redsleeve"] = "keep keep keep keep this this keep keep keep";
}

function ClothesSO::postEvents(%so)
{
	%str = "list";
	
	for(%a = 1; %so.str[%a] !$= ""; %a++)
		%str = %str SPC %so.uiName[%a] SPC %a;
	
	if(%str !$= "")
	{
		registerOutputEvent("fxDTSBrick", "RPsellClothes", %str TAB "int 0 500 1");
		
		for(%b = 0; %b < ClientGroup.getCount(); %b++)
		{
			%subClient = ClientGroup.getObject(%b);
			serverCmdRequestEventTables(%subClient);
			messageClient(%subClient, '', "\c6Your Event Tables have been updated. If you do not know what that means, ignore this message.");
		}
	}
}

function ClothesSO::getColor(%so, %client, %item)
{
	if(%item $= "skin" || %item $= "skingen")
		return %client.headColor;
	else
	{
		%color = %so.color[%item];
		return %color;
	}
}

function ClothesSO::getNode(%so, %client, %item)
{
	if(%item $= "skin")
		return 0;
	else if(%item $= "skingen")
		return (%client.gender $= "Male" ? 0 : 1);
	else
	{
		%node = %so.node[%item];
	
		if(%node $= "gender" || %node $= "skingen")
			return (%client.gender $= "Male" ? 0 : 1);
		else
			return %node;
	}
}

function ClothesSO::getDecal(%so, %client, %segment, %item)
{
	if(%item $= "" || %item $= "default")
	{
		if(%segment $= "face")
			return "smiley";
		else if(%segment $= "chest")
			return "AAA-none";
	}
	else
		return %item;
}

function ClothesSO::giveItem(%so, %client, %item)
{
	if(strLen(%so.str[%item]) && isObject(%client))
	{
		%outfit = %client.RPData.Value["outfit"];//CityRPGXData.getData(%client.bl_id).valueOutfit;
		
		for(%a = 0; %a < getWordCount(%outfit); %a++)
		{
			if(getWord(%so.str[%item], %a) $= "keep")
				%newOutfit = (%newOutfit $= "" ? getWord(%outfit, %a) : %newOutfit SPC getWord(%outfit, %a));
			else if(getWord(%so.str[%item], %a) $= "this")
				%newOutfit = (%newOutfit $= "" ? %item : %newOutfit SPC %item);
			else
				%newOutfit = (%newOutfit $= "" ? getWord(%so.str[%item], %a) : %newOutfit SPC getWord(%so.str[%item], %a));
		}
		
		%client.RPData.Value["outfit"] = %newOutfit;//CityRPGXData.getData(%client.bl_id).valueOutfit = %newOutfit;
		%client.applyBodyParts();
		%client.applyBodyColors();
	}
}

if(!isObject(ClothesSO))
{
	new scriptObject(ClothesSO) { };
	ClothesSO.schedule(1, "loadClothes");
	ClothesSO.schedule(1, "postEvents");
}

if (isPackage(RP_Jobs_Clothes))
	deactivatePackage(RP_Jobs_Clothes);
	
package RP_Jobs_Clothes
{
	function serverCmdUpdateBodyColors(%client, %headColor)
	{
		// The only thing we want from this command is the facial color, which determines skin color in the clothing mod.
		%client.headColor = %headColor;
		%client.applyForcedBodyColors();
	}
};
RPRegPackage(RP_Jobs_Clothes);

function fxDTSBrick::RPsellClothes(%brick, %item, %markup, %client)
{
	if(isObject(%client.player) && !%client.player.serviceOrigin  && isObject(%brick))
	{
		%client.player.serviceType = "clothes";
		%client.player.serviceItem = %item;
		%client.player.serviceFee = %markup;
		%client.player.serviceMarkup = %markup;
		%client.player.serviceOrigin = %brick;
		
		messageClient(%client,'', '\c6A clothing service is offering to dress you in a(n) \c3%1 \c6for \c3$%2\c6.', ClothesSO.sellName[%item], %client.player.serviceFee);
		messageClient(%client,'', "\c6Accept with \c3/yes\c6, decline with \c3/no\c6.");
	}
	else if(%client.player.serviceOrigin && %client.player.serviceOrigin != %brick)
		messageClient(%client, '', "\c6You already have a charge request from another service! Type \c3/no\c6 to reject it.");
}

function gameConnection::applyForcedBodyColors(%client)
{
	if(%client.RPData.Value["clothed"] == 1)
	{
		if(%client.RPData.Value["jail"] >= 1)
			%outfit = "none none none none jumpsuit jumpsuit skin jumpsuit jumpsuit";
		else if($RP::job::outfit[%client.RPData.Value["jobID"]] !$= "")
			%outfit = $RP::job::outfit[%client.RPData.Value["jobID"]];
		else
			%outfit = %client.RPData.Value["outfit"];
		
		if(%outfit !$= "")
		{
			%client.accentColor		= ClothesSO.getColor(%client, getWord(%outfit, 0));
			%client.hatColor		= ClothesSO.getColor(%client, getWord(%outfit, 1));
		
			%client.packColor		= ClothesSO.getColor(%client, getWord(%outfit, 2));
			%client.secondPackColor		= ClothesSO.getColor(%client, getWord(%outfit, 3));
		
			%client.chestColor		= ClothesSO.getColor(%client, getWord(%outfit, 4));
		
			%client.rarmColor		= ClothesSO.getColor(%client, getWord(%outfit, 5));
			%client.larmColor		= ClothesSO.getColor(%client, getWord(%outfit, 5));
			%client.rhandColor		= ClothesSO.getColor(%client, getWord(%outfit, 6));
			%client.lhandColor		= ClothesSO.getColor(%client, getWord(%outfit, 6));
		
			%client.hipColor		= ClothesSO.getColor(%client, getWord(%outfit, 7));
		
			%client.rlegColor		= ClothesSO.getColor(%client, getWord(%outfit, 8));
			%client.llegColor		= ClothesSO.getColor(%client, getWord(%outfit, 8));
		
			%client.applyBodyColors();
		}
	}
}


function gameConnection::applyForcedBodyParts(%client)
{
	if(%client.RPData.Value["clothed"] == 1)
	{
		if(isObject(%client))
		{
			if(%client.RPData.Value["jail"] >= 1)
				%outfit = "none none none none jumpsuit jumpsuit skin jumpsuit jumpsuit";
			else if($RP::job::outfit[%client.RPData.Value["jobID"]] !$= "")
				%outfit = $RP::job::outfit[%client.RPData.Value["jobID"]];
			else
				%outfit = %client.RPData.Value["outfit"];
		}
	
		if(%outfit !$= "")
		{	
			%client.accent		= ClothesSO.getNode(%client, getWord(%outfit, 0));
			%client.hat		= ClothesSO.getNode(%client, getWord(%outfit, 1));
		
			%client.pack		= ClothesSO.getNode(%client, getWord(%outfit, 2));
			%client.secondPack	= ClothesSO.getNode(%client, getWord(%outfit, 3));
		
			%client.chest		= ClothesSO.getNode(%client, getWord(%outfit, 4));
		
			%client.rarm		= ClothesSO.getNode(%client, getWord(%outfit, 5));
			%client.larm		= ClothesSO.getNode(%client, getWord(%outfit, 5));
			%client.rhand		= ClothesSO.getNode(%client, getWord(%outfit, 6));
			%client.lhand		= ClothesSO.getNode(%client, getWord(%outfit, 6));
		
			%client.hip		= ClothesSO.getNode(%client, getWord(%outfit, 7));
		
			%client.rleg		= ClothesSO.getNode(%client, getWord(%outfit, 8));
			%client.lleg		= ClothesSO.getNode(%client, getWord(%outfit, 8));

		
			%client.faceName 	= ClothesSO.getDecal(%client, "face", getWord(%outfit, 9));
			%client.decalName 	= ClothesSO.getDecal(%client, "chest", getWord(%outfit, 10));
		
			%client.applyBodyParts();
		}
	}
}

function gameConnection::sellClothes(%client, %sellerID, %brick, %item, %price)
{
	if(isObject(%client.player) && %client.RPData.value["Money"] >= %price)
	{
		if($RP::job::sellClothes[%client.RPData.value["jobID"]])
		{
			messageClient(%client, '', "\c6Enjoy the new look!");
			%client.RPData.value["Money"] -= %price;
			ClothesSO.giveItem(%client, %item);

			if(%price)
			{
				if(isObject(%seller = FindClientByBL_ID(%sellerID)))
				{
					messageClient(%seller, '', '\c6You just gained \c3$%1\c6 for selling clothes to \c3%2\c6.', %price, %client.name);
					%client.RPData.value["Bank"] += %price;
				}
			}			

			%client.applyForcedBodyColors();
			%client.applyForcedBodyParts();
			
			%client.setInfo();
		}
		else
			messageClient(%client, '', "\c6This vendor is not liscensed to sell clothes.");
	}
}

function serverCmdYes(%client)
{
	if(isObject(%client.player) && isObject(%client.player.serviceOrigin))
	{
		if(mFloor(VectorDist(%client.player.serviceOrigin.getPosition(), %client.player.getPosition())) < 16)
		{
			if(%client.RPData.value["Money"] >= %client.player.serviceFee)
			{
				%ownerBL_ID = %client.player.serviceOrigin.getGroup().bl_id;
				switch$(%client.player.serviceType)
				{
					case "service":
						%client.RPData.value["Money"] -= %client.player.serviceFee;
						%client.player.serviceOrigin.getGroup().RPData.value["Bank"] += %client.player.serviceFee;
						
						messageClient(%client, '', "\c6You have accepted the service fee of \c3$" @ %client.player.serviceFee @ "\c6!");
						%client.setInfo();
						
						if(%client.player.serviceOrigin.getGroup().client)
							messageClient(%client.player.serviceOrigin.getGroup().client, '', "\c3" @ %client.name @ "\c6 has wired you \c3$" @ %client.player.serviceFee @ "\c6 for a service.");
					
						%client.player.serviceOrigin.onTransferSuccess(%client);
						
					case "food":
						%client.sellFood(%ownerBL_ID, %client.player.serviceSize, %client.player.serviceItem, %client.player.serviceFee, %client.player.serviceMarkup);
						
					case "item":
						%client.sellItem(%ownerBL_ID, %client.player.serviceItem, %client.player.serviceFee, %client.player.serviceMarkup);
						
					case "zone":
						%client.sellZone(%ownerBL_ID, %client.player.serviceOrigin, %client.player.serviceFee);
						
					case "clothes":
						%client.sellClothes(%ownerBL_ID, %client.player.serviceOrigin, %client.player.serviceItem, %client.player.serviceFee);
				}
			}
			else
			{
				messageClient(%client, '', "\c6You cannot afford this service.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6You are too far away from the service to purchase it!");
		}
	}
	else
	{
		messageClient(%client, '', "\c6You have no active tranfers that you may accept!");
	}
	
	%client.player.serviceType = "";
	%client.player.serviceFee = "";
	%client.player.serviceMarkup = "";
	%client.player.serviceItem = "";
	%client.player.serviceSize = "";
	%client.player.serviceOrigin = "";
}

function serverCmdNo(%client)
{
	if(isObject(%client.player.serviceOrigin))
	{
		messageClient(%client, '', "\c6You have rejected the service fee!");
		
		%client.player.serviceOrigin.onTransferDecline(%client);
		
		%client.player.serviceType = "";
		%client.player.serviceFee = "";
		%client.player.serviceMarkup = "";
		%client.player.serviceItem = "";
		%client.player.serviceSize = "";
		%client.player.serviceOrigin = "";
	}
	else
		messageClient(%client, '', "\c6You have no active tranfers that you may decline!");
}

function serverCmdSexChange(%client)
{
	if(%client.RPData.value["gender"] !$= "Female")
		%client.RPData.value["gender"] = "Female";
		//CityRPGXData.getData(%client.bl_id).valueGender = "Female";
	else
		%client.RPData.value["gender"] = "Male";
		//CityRPGXData.getData(%client.bl_id).valueGender = "Male";
	
	%client.gender = %client.RPData.value["gender"];//CityRPGXData.getData(%client.bl_id).valueGender;
	%client.redoAvatar = true;
	
	%client.applyForcedBodyParts();
	%client.applyForcedBodyColors();
	
	messageClient(%client, '', '\c6You are now \c3%1\c6.', %client.RPData.value["Gender"]);
}

function serverCmdUniformChange(%client)
{
	if(%client.RPData.value["clothed"] != 0)
	{
		%client.RPData.value["clothed"] = 0;
		//CityRPGXData.getData(%client.bl_id).valueGender = "Female";
	}
	else
		%client.RPData.value["clothed"] = 1;
		//CityRPGXData.getData(%client.bl_id).valueGender = "Male";
	
	%client.applyForcedBodyParts();
	%client.applyForcedBodyColors();
	
	messageClient(%client, '', '\c6You are now \c3%1\c6.', %client.RPData.value["clothed"]);
}
