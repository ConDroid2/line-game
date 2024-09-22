/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID BREAK_ALLBGSSWITCHCONTAINER = 2929608979U;
        static const AkUniqueID MUTE_ALLBGSSWITCHCONTAINER = 2649641287U;
        static const AkUniqueID PAUSE_ALLBGSSWITCHCONTAINER = 346159844U;
        static const AkUniqueID PLAY_ALLBGSSWITCHCONTAINER = 3850123198U;
        static const AkUniqueID PLAY_MEMORY_LEAK_FOLEY = 3719212787U;
        static const AkUniqueID PLAY_WANDERING = 465142343U;
        static const AkUniqueID RESET_GAME_PARAMETER_MUSICVOLUME = 2122702989U;
        static const AkUniqueID RESET_VOICE_HIGH_PASS_FILTER_ALLBGSSWITCHCONTAINER = 2238057352U;
        static const AkUniqueID RESET_VOICE_LOW_PASS_FILTER_ALLBGSSWITCHCONTAINER = 2103622306U;
        static const AkUniqueID RESET_VOICE_VOLUME_ALLBGSSWITCHCONTAINER = 2795130315U;
        static const AkUniqueID RESUME_ALLBGSSWITCHCONTAINER = 2148965273U;
        static const AkUniqueID SEEK_ALLBGSSWITCHCONTAINER = 3083749424U;
        static const AkUniqueID SET_GAME_PARAMETER_MUSICVOLUME = 3291076718U;
        static const AkUniqueID SET_SWITCH_BEEPSOFF = 1745318411U;
        static const AkUniqueID SET_SWITCH_BEEPSON = 1112751095U;
        static const AkUniqueID SET_SWITCH_DANGERVORTEXOFF = 2427286275U;
        static const AkUniqueID SET_SWITCH_DANGERVORTEXON = 3862409775U;
        static const AkUniqueID SET_VOICE_HIGH_PASS_FILTER_ALLBGSSWITCHCONTAINER = 1742419299U;
        static const AkUniqueID SET_VOICE_LOW_PASS_FILTER_ALLBGSSWITCHCONTAINER = 998656967U;
        static const AkUniqueID SET_VOICE_VOLUME_ALLBGSSWITCHCONTAINER = 3223721356U;
        static const AkUniqueID STOP_ALLBGSSWITCHCONTAINER = 1661072716U;
        static const AkUniqueID STOP_MEMORY_LEAK_FOLEY = 1429974361U;
        static const AkUniqueID STRUGGLEBEAT1 = 1952352785U;
        static const AkUniqueID STRUGGLEBEAT2 = 1952352786U;
        static const AkUniqueID STRUGGLEBEAT3 = 1952352787U;
        static const AkUniqueID STRUGGLEBEAT4 = 1952352788U;
        static const AkUniqueID STRUGGLEBEAT5 = 1952352789U;
        static const AkUniqueID UNMUTE_ALLBGSSWITCHCONTAINER = 777545546U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace WANDERINGPAD
        {
            static const AkUniqueID GROUP = 2789638893U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SAWPAD = 3843366911U;
                static const AkUniqueID SQUAREANDSAWPAD = 309159965U;
                static const AkUniqueID SQUAREPADWETONLY = 3547219115U;
                static const AkUniqueID SQUREPADDRYONLY = 1730641605U;
            } // namespace STATE
        } // namespace WANDERINGPAD

        namespace WANDERINGPERCUSSION
        {
            static const AkUniqueID GROUP = 4286244013U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace WANDERINGPERCUSSION

        namespace WORLDTRACK
        {
            static const AkUniqueID GROUP = 3670271356U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace WORLDTRACK

    } // namespace STATES

    namespace SWITCHES
    {
        namespace BACKGROUNDTRACK
        {
            static const AkUniqueID GROUP = 693291814U;

            namespace SWITCH
            {
                static const AkUniqueID MICROPROCESSOR_W1 = 469249852U;
                static const AkUniqueID SOONFORGOTTEN_W3 = 4089353215U;
                static const AkUniqueID STRUGGLE_W4 = 755231598U;
                static const AkUniqueID UNAUTHORIZED_W2 = 1491136465U;
                static const AkUniqueID WANDERING_W0 = 2818412452U;
            } // namespace SWITCH
        } // namespace BACKGROUNDTRACK

        namespace COMPUTERFOLEYFORDEPTHS
        {
            static const AkUniqueID GROUP = 1800164242U;

            namespace SWITCH
            {
                static const AkUniqueID BEEPSOFF = 3163588159U;
                static const AkUniqueID BEEPSON = 3664575467U;
            } // namespace SWITCH
        } // namespace COMPUTERFOLEYFORDEPTHS

        namespace DANGERVORTEXSWITCH
        {
            static const AkUniqueID GROUP = 2095248444U;

            namespace SWITCH
            {
                static const AkUniqueID DANGERVORTEXOFF = 1811042591U;
                static const AkUniqueID DANGERVORTEXON = 1813477451U;
            } // namespace SWITCH
        } // namespace DANGERVORTEXSWITCH

        namespace UNAUTHORIZED
        {
            static const AkUniqueID GROUP = 235446319U;

            namespace SWITCH
            {
                static const AkUniqueID DEFAULT = 782826392U;
                static const AkUniqueID LIGHTSOUT = 702027158U;
            } // namespace SWITCH
        } // namespace UNAUTHORIZED

        namespace W3_ZONE
        {
            static const AkUniqueID GROUP = 1936950428U;

            namespace SWITCH
            {
                static const AkUniqueID DEPTHS = 246282379U;
                static const AkUniqueID UPPERLEVEL = 779567659U;
            } // namespace SWITCH
        } // namespace W3_ZONE

        namespace WANDERIGNSQUAREPAD
        {
            static const AkUniqueID GROUP = 3053675412U;

            namespace SWITCH
            {
                static const AkUniqueID OFF = 930712164U;
                static const AkUniqueID ON = 1651971902U;
                static const AkUniqueID ONWITH8VADOUBLE = 1066491206U;
            } // namespace SWITCH
        } // namespace WANDERIGNSQUAREPAD

        namespace WANDERINGARPEGGIO
        {
            static const AkUniqueID GROUP = 2086391272U;

            namespace SWITCH
            {
                static const AkUniqueID OFF = 930712164U;
                static const AkUniqueID SAW = 443572616U;
                static const AkUniqueID SQUAREDRY = 127960571U;
                static const AkUniqueID SQUAREWITHDELAY = 3696409429U;
            } // namespace SWITCH
        } // namespace WANDERINGARPEGGIO

        namespace WANDERINGBASS
        {
            static const AkUniqueID GROUP = 2812095919U;

            namespace SWITCH
            {
                static const AkUniqueID ARPINBASS = 2829385960U;
                static const AkUniqueID DISCOBASS = 114451572U;
                static const AkUniqueID OFF = 930712164U;
            } // namespace SWITCH
        } // namespace WANDERINGBASS

        namespace WANDERINGMELODY
        {
            static const AkUniqueID GROUP = 1412217666U;

            namespace SWITCH
            {
                static const AkUniqueID DISCOMELODY = 421134689U;
                static const AkUniqueID OFF = 930712164U;
                static const AkUniqueID SAW = 443572616U;
                static const AkUniqueID SQUARE = 1818333208U;
            } // namespace SWITCH
        } // namespace WANDERINGMELODY

        namespace WANDERINGPERCUSSION
        {
            static const AkUniqueID GROUP = 4286244013U;

            namespace SWITCH
            {
                static const AkUniqueID A = 84696446U;
                static const AkUniqueID B = 84696445U;
                static const AkUniqueID C = 84696444U;
                static const AkUniqueID D = 84696443U;
                static const AkUniqueID OFF = 930712164U;
            } // namespace SWITCH
        } // namespace WANDERINGPERCUSSION

        namespace WANDERINGPULSEWAVE
        {
            static const AkUniqueID GROUP = 1421364506U;

            namespace SWITCH
            {
                static const AkUniqueID DISCO = 3920460409U;
                static const AkUniqueID OFF = 930712164U;
            } // namespace SWITCH
        } // namespace WANDERINGPULSEWAVE

        namespace WANDERINGSAWPAD
        {
            static const AkUniqueID GROUP = 1528543176U;

            namespace SWITCH
            {
                static const AkUniqueID OFF = 930712164U;
                static const AkUniqueID ON = 1651971902U;
            } // namespace SWITCH
        } // namespace WANDERINGSAWPAD

        namespace WANDERINGSUBTRACK
        {
            static const AkUniqueID GROUP = 1243382953U;

            namespace SWITCH
            {
                static const AkUniqueID BASE = 1291433366U;
                static const AkUniqueID DISCO = 3920460409U;
                static const AkUniqueID GROOVE = 2129540867U;
                static const AkUniqueID MELODY_SQUARE = 1643365445U;
            } // namespace SWITCH
        } // namespace WANDERINGSUBTRACK

        namespace ZONE
        {
            static const AkUniqueID GROUP = 832057375U;

            namespace SWITCH
            {
                static const AkUniqueID A = 84696446U;
                static const AkUniqueID B = 84696445U;
                static const AkUniqueID C = 84696444U;
                static const AkUniqueID D = 84696443U;
                static const AkUniqueID E = 84696442U;
                static const AkUniqueID F = 84696441U;
            } // namespace SWITCH
        } // namespace ZONE

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID MUSICVOLUME = 2346531308U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID BACKGROUNDMUSICSOUNDBANK = 3618373U;
        static const AkUniqueID TESTSOUNDBANK = 1831431028U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
