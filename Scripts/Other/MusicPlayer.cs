using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Prefab]
public class MusicPlayer : Singleton<MusicPlayer>
{
    [SerializeField]
    private AudioClip[] tracks;

    private List<AudioSource> trackSources;
    private List<int> activeTracks = new List<int>();
    private int currentTrack = -1;
    private bool swappingTrack = false;
    private float currentTime = 0f;
    private float timeToChange = 2f;

    private void Awake()
    {
        InitializeMusicPlayer(this.gameObject);
    }

    public void InitializeMusicPlayer(GameObject gameObject)
    {
        trackSources = new List<AudioSource>();

        foreach (var track in tracks)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = track;
            source.loop = true;
            source.playOnAwake = false;

            trackSources.Add(source);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (swappingTrack)
        {
            if (currentTime <= timeToChange)
            {
                currentTime += Time.deltaTime;

                foreach (var track in activeTracks)
                {
                    trackSources[track].volume = (track == currentTrack) ? Mathf.Lerp(0.0f, 0.5f, currentTime / timeToChange) : Mathf.Lerp(0.5f, 0.0f, currentTime / timeToChange);
                }
            }
            else
            {
                currentTime = 0.0f;
                swappingTrack = false;
            }
        }
    }

    public void PlayTrack(int trackNumber, float volume = 0.5f)
    {
        if (!this.activeTracks.Contains(trackNumber))
        {
            StopTracks();
            activeTracks.Add(trackNumber);
            currentTrack = trackNumber;
            trackSources[trackNumber].volume = volume;
            trackSources[trackNumber].Play();
        }
    }

    public void PlaySimultaneousTracks(int mainTrackNumber, params int[] allTracks)
    {
        var uniqueTracks = new HashSet<int>();
        StopTracks();

        // Add tracks to set to prevent repeat tracks
        uniqueTracks.Add(mainTrackNumber);
        foreach (var track in allTracks)
        {
            uniqueTracks.Add(track);
        }

        activeTracks.AddRange(uniqueTracks);

        foreach (var track in uniqueTracks)
        {
            trackSources[track].volume = (track == mainTrackNumber) ? 0.5f : 0.0f;
            trackSources[track].Play();
        }
        currentTrack = mainTrackNumber;
    }

    public void StopTracks()
    {
        foreach (var activeTrack in activeTracks)
        {
            trackSources[activeTrack].Stop();
        }

        activeTracks.Clear();
    }

    public void SwapActiveTrack(int activeTrack)
    {
        currentTrack = activeTrack;
        swappingTrack = true;
    }
}
