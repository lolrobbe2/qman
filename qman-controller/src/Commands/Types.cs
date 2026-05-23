using System;
using System.Collections.Generic;
using System.Text;

namespace qman.controller.src.Commands
{
public enum XPORT_BROADCAST_COMMANDS : byte
{
    // --- System / Basic ---
    NODE_RESET = 3,    // 0x03: Forces a soft reboot
    NODE_FIND = 63, //0x3f
    ERROR_RESPONSE = 255,  // 0xFF: Returned if a command is malformed

    // --- Discovery & Versioning ---
    FIRMWARE_QUERRY = 246,  // 0xF6: "Question"
    FIRMWARE_RESPONSE = 247,  // 0xF7: "Answer" (Standard 26-byte response)

    EXTENDED_VERSION_QUERRY = 244,  // 0xF4: Request for long-form version info
    EXTENDED_VERSION_RESPONSE = 245,  // 0xF5: Detailed firmware string

    // --- Configuration (Setup Record 1 & 2) ---
    // Record 2 is the most common for modern XPort discovery
    SETUP_RECORD_2_QUERRY = 226,  // 0xE2: Search for devices (often used with 0x00 payload)
    SETUP_RECORD_2_RESPONSE = 210,  // 0xD2: Returns IP, MAC, Name, and Port config

    SETUP_RECORD_1_QUERRY = 225,  // 0xE1: Legacy search
    SETUP_RECORD_1_RESPONSE = 209,  // 0xD1: Legacy response

    // --- IP Assignment (Address Resolution) ---
    SET_IP_ADDRESS_QUERRY = 248,  // 0xF8: Assigns a temporary IP based on MAC
    SET_IP_ADDRESS_RESPONSE = 249,  // 0xF9: Confirmation of IP assignment

    // --- Security & Protection ---
    PASSWORD_QUERRY = 224,  // 0xE0: Used for encrypted/password discovery
    PASSWORD_RESPONSE = 208,  // 0xD0: Password status

    // --- Advanced / Proprietary ---
    SCAN_WLAN_QUERRY = 212,  // 0xD4: WiPort specific: Scan for Access Points
    SCAN_WLAN_RESPONSE = 196,   // 0xC4: WiPort specific: AP List

    IP_SETUP = 252
    }
}
