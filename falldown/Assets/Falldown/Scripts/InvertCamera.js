
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/


#pragma strict

 function OnPreCull () {
     GetComponent.<Camera>().ResetWorldToCameraMatrix ();
     GetComponent.<Camera>().ResetProjectionMatrix ();
     GetComponent.<Camera>().projectionMatrix = GetComponent.<Camera>().projectionMatrix * Matrix4x4.Scale(Vector3 (1, -1, 1));
 }
  
 function OnPreRender () {
     GL.SetRevertBackfacing (true);
 }
  
 function OnPostRender () {
     GL.SetRevertBackfacing (false);
 }
  
 @script RequireComponent (Camera)