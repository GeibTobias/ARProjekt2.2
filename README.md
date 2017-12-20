# Interactive 3D City Roadtrip Planner
## Abstract
The goal of this application was to build a tool which inhaces the planning of a roadtrip by using mobile sensors as input and a large projector as output in a Augmented Reality invornment.

In this application a map will be projected onto a table. This map has serveral points of interests marked with a symbol. Multiple users can now, by opening a secondary application, use their mobile phones to see 3D models in augmented reality placed in perspective of the points of interest on the projected map. By clicking on the 3D object, the user can now add the respective POI to the list of places to visit on the roadtrip. When more than one POIs are added the projected map will show the route of the roadtrip indicated by a redline. Up to 10 POIs can be added and removed. Each user has the option of navigating the map by either opening a navigation menu or by pressing a navigation button, which allows to adjust the center view point of the map by tilting the device to the disered direction. The navigation menu also has the option to zoom in and out of the map, which may cause for fewer POIs to be displayed in order to avoid cluttering. 

## Implementation details
The system is made up of 3 subsystems. The mobile system which displays the 3D objects in augmented reality space, the map which is projected from a desktop machine and the server which connects the map with each users mobile system. 

The mobile system utilizes the Vuforia application in order to detect and place the 3D objects appropriately. The map has uses Vumarks as marker for each POI, thus the mobile app will detect each POI as an individual marker. By clicking on the mobile screen a raycast will detect which 3D model was clicked on. Adding or removing a POI will send the relevant information to the server which will then update the POI list for every user. When the state of the map is changed by one user by navigating or zooming, the server will also update the map accordingly. 

In order for the system to work the ip-adress of the hosting computer must be configured.

![alt text](https://github.com/GeibTobias/ARProjekt2.2/blob/master/roadtrip.jpg "Roadtrip")
