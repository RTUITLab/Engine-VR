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
        foreach (var info in networking.cachedRoomList)
        {
            /*int index = listings.FindIndex(x => x.RoomInfo.Name == info.Value.Name);
            if (index != -1)
            {
                Destroy(listings[index].gameObject);
                listings.RemoveAt(index);
            }
            else
            { */
            RoomListing listing = Instantiate(roomListing, content);
            if (listing != null)
            {
                listing.SetRoomInfo(info.Value);
                listings.Add(listing);
                /*}
            }*/
            }
        }
    }

    private void OnDisable()
    {
        foreach (var info in listings)
        {
            Destroy(info.gameObject);
        }
    }
}