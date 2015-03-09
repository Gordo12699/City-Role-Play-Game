// --------------------------
// RP Core
// --------------------------

echo("RP Core: Creating instances");

$RP::activated = false;

// Run scripts
exec("./system.cs");
exec("./save.cs");
exec("./minigame.cs");
exec("./bricks.cs");
exec("./game.cs");
exec("./user.cs");
exec("./commands.cs");
exec("./events.cs");
exec("./guicontrols.cs");
exec("./cellphones.cs");

// And the last stuff
exec("./initload.cs");
