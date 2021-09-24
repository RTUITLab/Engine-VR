using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class RoomListingsMenu : MonoBehaviour
{
    [SerializeField] private Networking networking;
    [SerializeField] private Transform content;
    [SerializeField] private RoomListing roomListing;

    private List<RoomListing> listings = new List<RoomListing>();

    public void UpdateRoomListing()
    {
        listings.Clear();

        foreach (RoomListing old in content.GetComponentsInChildren<RoomListing>())
        {
            Destroy(old.gameObject);
        }

        foreach (var info in networking.cachedRoomList)
        {
            RoomListing listing = Instantiate(roomListing, content);
            if (listing != null)
            {
                listing.SetRoomInfo(info.Value);
                listings.Add(listing);
            }
        }
    }

    private void OnDisable()
    {
        foreach (var info in listings)
        {
            Destroy(info.gameObject);
            listings.Remove(info);
        }
    }
}