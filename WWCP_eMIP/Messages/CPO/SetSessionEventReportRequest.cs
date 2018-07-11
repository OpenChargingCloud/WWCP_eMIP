

// Report an event from a CPO during a charging session.

// eventReportCode
// 
// Code  Signification
//   0   stopped: Emergency Stop
//   1   operation terminated
//   2   operation suspended
//   3   operation started
//  11   Start of charge
//  12   End of charge


//     1  Ok-Normal  Normal successful completion 
// 10501  Ko-Error   session not found
// 10502  Ko-Error   CPO/eMSP not found
// 10503  Ko-Error   the CPO/eMSP does not accept Action/Event
// 10504  Ko-Error   the request cannot be sent to the CPO/eMSP or the CPO/eMSP does not respond
// 10505  Ko-Error   the CPO/eMSP returns an IOP Fault 
// 10506  Ko-Error   the  CPO/eMSP  doesn't  recognise  the  actionNature/eventNature:  No action on its side
// 10507  Ko-Error   the CPO/eMSP returns an error code: an error occured on its side during the action/report treatment 
// 10508  Ko-Error   The requestor is neither eMSP nor CPO for this session.
// 
// Others <  10 000  Ok  Reserved for future use
// Others >= 10 000  Ko-Error  Reserved for future use

