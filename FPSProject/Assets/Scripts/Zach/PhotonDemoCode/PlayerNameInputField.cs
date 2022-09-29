using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

	[RequireComponent(typeof(TMP_InputField))]
	public class PlayerNameInputField : MonoBehaviour
	{
		#region Private Constants

		// Store the PlayerPref Key to avoid typos
		const string playerNamePrefKey = "PlayerName";

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during initialization phase.
		/// </summary>
		void Start()
		{

			string defaultName = "DefaultName";
			TMP_InputField _inputField = this.GetComponent<TMP_InputField>();

			if (_inputField != null)
			{
				if (PlayerPrefs.HasKey(playerNamePrefKey))
				{
					defaultName = PlayerPrefs.GetString(playerNamePrefKey);
					_inputField.text = defaultName;
				}
			}

			PhotonNetwork.NickName = defaultName;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
		/// </summary>
		/// <param name="value">The name of the Player</param>
		public void SetPlayerName(string value)
		{
			// #Important
			if (string.IsNullOrEmpty(value))
			{
				Debug.LogError("Player Name is null or empty");
				return;
			}
			PhotonNetwork.NickName = value;

			PlayerPrefs.SetString(playerNamePrefKey, value);
		}

		#endregion
	}
