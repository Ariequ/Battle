using UnityEngine;
using System.Collections;

internal class MessageConfig {

	public const int MIN_MSG_LEN = 10;
	public const int MSG_ID_LEN = 4;
	public const int MAX_MSG_LEN = 256;
	
	public const int MSG_HEADER_LEN = 2;
	public const int MSG_BODY_LEN = 4;

	public const int HEADER_LEN_OFFSET = MessageConfig.MSG_ID_LEN + 4;
}
