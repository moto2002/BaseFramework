//the audio clip we want to play
var audioTrack : AudioClip;
//the listener (main camera for this example)
var listener : AudioListener;
//the number of samples we want to take
var sampleRate : float = 256;
//how often we want to take a sample
var timeSpace : float= 0.2;
//the game object we will be using to represent our samples
var visualPrefab : GameObject;
 
private var volData : float[];
private var freqData : float[];
private var numSamples : int;
//we'll move along the z plane when placing our objects
private var curZ : int = 0;
//we'll clamp our positions between these values to avoid erratic placements
private var maxX : float = 5.0;
private var minX : float = -5.0;
 
function Start() {
 
	//create audio player game object and position it at the same point as our audio listener
	audioPlay = new GameObject("audioPlay");
	audioPlay.AddComponent("AudioSource");
	audioPlay.transform.position = listener.transform.position;
	audioPlay.audio.clip = audioTrack;
	audioPlay.audio.Play();
 
	//prep our number of samples, we clamp it between 64 and 8192 since this is the min and the max for the numSamples argument
	numSamples = Mathf.Clamp(sampleRate * timeSpace, 64, 8192);
 
	//prep our float arrays
	volData = new float[numSamples];
	freqData = new float[numSamples];
 
	InvokeRepeating("PlaceNewObject", 0, timeSpace);
 
}
 
function PlaceNewObject() {
	//update z position
	curZ += 2;
 
	//get the output data from the listener
	listener.GetOutputData(volData, 0);
 
	//get the root mean square of the output data (this is the square root of the average of the samples)
	curVol = RMS(volData);
 
	//amplify the volume, and maintain our range of minX and maxX
	xPos = Mathf.Clamp(curVol * 100, minX, maxX);
 
	//only place a new object if we aren't at the extremes of our clamp values
	if (xPos != minX && xPos != -maxX) {
		//get the spectrum data from the listener (we use the blackman harris window for maximum contrast)
		listener.GetSpectrumData(freqData, 1, FFTWindow.BlackmanHarris);
 
		//get the root mean square of the spectrum data
		curFreq = RMS(freqData);
 
		//amplify the frequency for more visual impact
		yPos = Mathf.Clamp(curFreq * 200, 0, maxX);
 
		//instantiate our new visual object, adjusting x, y and z position as we go
		//newVisual = Instantiate(visualPrefab, Vector3(xPos, yPos, curZ), transform.rotation);
		visualPrefab.transform.position = new Vector3(visualPrefab.transform.position.x, yPos, curZ);
	}
 
}
 
function RMS(samples: float[]) {
	var result = 0.0;
	//add sample values together
	for (i = 0; i < samples.Length; i++) {
		result += samples[i] * samples[i];
	}
	//get the average of the sample values
	result /= samples.Length;
	//return the square root of the average
	return Mathf.Sqrt(result);
}