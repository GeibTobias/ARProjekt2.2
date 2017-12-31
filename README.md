# Interactive 3D City Roadtrip Planner
## Abstract
The goal of this application was to build a tool which inhaces the planning of a roadtrip by using mobile sensors as input and a large projector as output in an Augmented Reality environment.

In this application a map will be projected onto a table. This map has serveral points of interests marked with a symbol. Multiple users can now, by opening a secondary mobile application, use their smartphones to see 3D models in augmented reality placed in perspective of the points of interest on the projected map. By tapping on the 3D object, the user can add the respective POI to the list of places to visit on the roadtrip. When more than one POIs are added the projected map will show the shortest route connecting the listed POIs indicated by a redline. Up to 10 POIs can be added and removed. Each user has the option of changing the viewport of the map by either using a DPad in the navigation menu or by opening the navigation menu and tilting the device to the disered direction. The navigation menu also has the option to zoom in and out of the map, which may cause fewer or more POIs to be displayed in order to avoid cluttering.

## Implementation details
The system is made up of 3 subsystems. The mobile apps which displays the 3D objects in augmented reality, a WebApp which displays the map and runs on a desktop machine connected to a projector and the server which connects the map with each users mobile system.

The mobile system utilizes the Vuforia f in order to detect and place the 3D objects appropriately. The map uses VuMarks as marker for each POI, thus the mobile app will detect each POI as an individual marker. By clicking on the mobile screen a raycast will detect which 3D model was clicked on. Adding or removing a POI will send the relevant information to the server which will then update the POI list for on all clients. When the viewport of the map is changed by one user, the server will also send updates abour the new state to the client displaying the map and the smartphones.

In order for the system to work the IP adress of the hosting computer must be configured in the Unity project of the smartphone apps.

![alt text](https://github.com/GeibTobias/ARProjekt2.2/blob/master/roadtrip.jpg "Roadtrip")
