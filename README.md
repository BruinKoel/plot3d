Comppiled it on another machine and it seems to not work? here is the publish of master for sanity: https://drive.google.com/file/d/1nokwCSo3RjYtXkOgHfoD0ThlCI7I-ZCa/view?usp=sharing

WASD to fly, and you can click and drag the viewport to adjust your look direction.
The WPF setup is kinda sketch, but I didn't plan on having to build this much graphics code as it wasn't the goal of this project. not even sure why i am programming this in C# anymore.

The Goal of the project:
"To be determined"
Right now I am just testing the water of all this material, I take everything problem by problem, and whenever inspiritaion strikes i add some functionality.


Ideation phases:

phase 1, could i build a slicer? yes done. take an object slice it along the Z axis. 

phase 2 what if i used splines generated from the mesh, that way it's all smooth.

phase 3 already bored of finetuning my splines, besides ordering toolpaths has been done before, and better than i currently can. how can i do it different?

phase 4 what if i represent the volume instead of the srufaces of this object, True mathematical volumetric calculations based on my current code, will be too much effort but a tought for when I have the guts to jump in that rabbit hole.

current phase 5 voxels? or even more simple, field of scalar values? this feels good, because there could be a simple conversion to input variable for a model somewhere in there?

Where is the Output file, I tought this was a slicer!? converting the Curves to Gcode is not high on the list right now, maybe it will become points to gcode, maybe we only do RobTargets here. thanks for reading <3.
