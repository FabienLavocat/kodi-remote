﻿namespace KodiRemote.Core.Commands
{
    public enum PropertyNames
    {
        CanShutdown,
        CanSuspend,
        CanHibernate,
        CanReboot
    }

    public enum ListFieldsAll
    {
        title,
        artist,
        albumartist,
        genre,
        year,
        rating,
        album,
        track,
        duration,
        comment,
        lyrics,
        musicbrainztrackid,
        musicbrainzartistid,
        musicbrainzalbumid,
        musicbrainzalbumartistid,
        playcount,
        fanart,
        director,
        trailer,
        tagline,
        plot,
        plotoutline,
        originaltitle,
        lastplayed,
        writer,
        studio,
        mpaa,
        cast,
        country,
        imdbnumber,
        premiered,
        productioncode,
        runtime,
        set,
        showlink,
        streamdetails,
        top250,
        votes,
        firstaired,
        season,
        episode,
        showtitle,
        thumbnail,
        file,
        resume,
        artistid,
        albumid,
        tvshowid,
        setid,
        watchedepisodes,
        disc,
        tag,
        art,
        genreid,
        displayartist,
        albumartistid,
        description,
        theme,
        mood,
        style,
        albumlabel,
        sorttitle,
        episodeguide,
        uniqueid,
        dateadded,
        channel,
        channeltype,
        hidden,
        locked,
        channelnumber,
        starttime,
        endtime
    }

    public enum PlayerPropertyName
    {
        type,
        partymode,
        speed,
        time,
        percentage,
        totaltime,
        playlistid,
        position,
        repeat,
        shuffled,
        canseek,
        canchangespeed,
        canmove,
        canzoom,
        canrotate,
        canshuffle,
        canrepeat,
        currentaudiostream,
        audiostreams,
        subtitleenabled,
        currentsubtitle,
        subtitles,
        live
    }

    public enum PlayerRepeat
    {
        Off,
        One,
        All
    }

    public enum GoTos
    {
        Previous,
        Next
    }

    public enum Seeks
    {
        SmallForward,
        SmallBackward,
        BigForward,
        BigBackward
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public enum SortMethod
    {
        none,
        label,
        date,
        size,
        file,
        path,
        drivetype,
        title,
        track,
        time,
        artist,
        album,
        albumtype,
        genre,
        country,
        year,
        rating,
        votes,
        top250,
        programcount,
        playlist,
        episode,
        season,
        totalepisodes,
        watchedepisodes,
        tvshowstatus,
        tvshowtitle,
        sorttitle,
        productioncode,
        mpaa,
        studio,
        dateadded,
        lastplayed,
        playcount,
        listeners,
        bitrate,
        random
    }

    public enum AddonFields
    {
        name,
        version,
        summary,
        description,
        path,
        author,
        thumbnail,
        disclaimer,
        fanart,
        dependencies,
        broken,
        extrainfo,
        rating,
        enabled
    }

    public enum AddonEnabled
    {
        True,
        False,
        all
    }

    public enum AddonTypes
    {
        unknown,
        xbmc_metadata_scraper_albums,
        xbmc_metadata_scraper_artists,
        xbmc_metadata_scraper_movies,
        xbmc_metadata_scraper_musicvideos,
        xbmc_metadata_scraper_tvshows,
        xbmc_ui_screensaver,
        xbmc_player_musicviz,
        xbmc_python_pluginsource,
        xbmc_python_script,
        xbmc_python_weather,
        xbmc_python_subtitles,
        xbmc_python_lyrics,
        xbmc_gui_skin,
        xbmc_gui_webinterface,
        xbmc_addon_video,
        xbmc_addon_audio,
        xbmc_addon_image,
        xbmc_addon_executable,
        xbmc_service
    }

    public enum AddonContent
    {
        unknown,
        video,
        audio,
        image,
        executable
    }

    public enum AudioFieldsArtist
    {
        instrument,
        style,
        mood,
        born,
        formed,
        description,
        genre,
        died,
        disbanded,
        yearsactive,
        musicbrainzartistid,
        fanart,
        thumbnail
    }

    public enum AudioFieldsAlbum
    {
        title,
        description,
        artist,
        genre,
        theme,
        mood,
        style,
        type,
        albumlabel,
        rating,
        year,
        musicbrainzalbumid,
        musicbrainzalbumartistid,
        fanart,
        thumbnail,
        playcount,
        genreid,
        artistid,
        displayartist
    }

    public enum AudioFieldsSong
    {
        title,
        artist,
        albumartist,
        genre,
        year,
        rating,
        album,
        track,
        duration,
        comment,
        lyrics,
        musicbrainztrackid,
        musicbrainzartistid,
        musicbrainzalbumid,
        musicbrainzalbumartistid,
        playcount,
        fanart,
        thumbnail,
        file,
        albumid,
        lastplayed,
        disc,
        genreid,
        artistid,
        displayartist,
        albumartistid
    }

    public enum LibraryFieldsGenre
    {
        title,
        thumbnail
    }

    public enum VideoFieldsEpisode
    {
        title,
        plot,
        votes,
        rating,
        writer,
        firstaired,
        playcount,
        runtime,
        director,
        productioncode,
        season,
        episode,
        originaltitle,
        showtitle,
        cast,
        streamdetails,
        lastplayed,
        fanart,
        thumbnail,
        file,
        resume,
        tvshowid,
        dateadded,
        uniqueid,
        art
    }

    public enum VideoFieldsMovieSet
    {
        title,
        playcount,
        fanart,
        thumbnail,
        art
    }

    public enum VideoFieldsMovie
    {
        title,
        genre,
        year,
        rating,
        director,
        trailer,
        tagline,
        plot,
        plotoutline,
        originaltitle,
        lastplayed,
        playcount,
        writer,
        studio,
        mpaa,
        cast,
        country,
        imdbnumber,
        runtime,
        set,
        showlink,
        streamdetails,
        top250,
        votes,
        fanart,
        thumbnail,
        file,
        sorttitle,
        resume,
        setid,
        dateadded,
        tag,
        art
    }

    public enum VideoFieldsMusicVideo
    {
        title,
        playcount,
        runtime,
        director,
        studio,
        year,
        plot,
        album,
        artist,
        genre,
        track,
        streamdetails,
        lastplayed,
        fanart,
        thumbnail,
        file,
        resume,
        dateadded,
        tag,
        art
    }

    public enum VideoFieldsSeason
    {
        season,
        showtitle,
        playcount,
        episode,
        fanart,
        thumbnail,
        tvshowid,
        watchedepisodes,
        art
    }

    public enum VideoFieldsTVShow
    {
        title,
        genre,
        year,
        rating,
        plot,
        studio,
        mpaa,
        cast,
        playcount,
        episode,
        imdbnumber,
        premiered,
        votes,
        lastplayed,
        fanart,
        thumbnail,
        file,
        originaltitle,
        sorttitle,
        episodeguide,
        season,
        watchedepisodes,
        dateadded,
        tag,
        art
    }

    public enum InputActions
    {
        left,
        right,
        up,
        down,
        pageup,
        pagedown,
        select,
        highlight,
        parentdir,
        parentfolder,
        back,
        previousmenu,
        info,
        pause,
        stop,
        skipnext,
        skipprevious,
        fullscreen,
        aspectratio,
        stepforward,
        stepback,
        bigstepforward,
        bigstepback,
        osd,
        showsubtitles,
        nextsubtitle,
        codecinfo,
        nextpicture,
        previouspicture,
        zoomout,
        zoomin,
        playlist,
        queue,
        zoomnormal,
        zoomlevel1,
        zoomlevel2,
        zoomlevel3,
        zoomlevel4,
        zoomlevel5,
        zoomlevel6,
        zoomlevel7,
        zoomlevel8,
        zoomlevel9,
        nextcalibration,
        resetcalibration,
        analogmove,
        rotate,
        rotateccw,
        close,
        subtitledelayminus,
        subtitledelay,
        subtitledelayplus,
        audiodelayminus,
        audiodelay,
        audiodelayplus,
        subtitleshiftup,
        subtitleshiftdown,
        subtitlealign,
        audionextlanguage,
        verticalshiftup,
        verticalshiftdown,
        nextresolution,
        audiotoggledigital,
        number0,
        number1,
        number2,
        number3,
        number4,
        number5,
        number6,
        number7,
        number8,
        number9,
        osdleft,
        osdright,
        osdup,
        osddown,
        osdselect,
        osdvalueplus,
        osdvalueminus,
        smallstepback,
        fastforward,
        rewind,
        play,
        playpause,
        delete,
        copy,
        move,
        mplayerosd,
        hidesubmenu,
        screenshot,
        rename,
        togglewatched,
        scanitem,
        reloadkeymaps,
        volumeup,
        volumedown,
        mute,
        backspace,
        scrollup,
        scrolldown,
        analogfastforward,
        analogrewind,
        moveitemup,
        moveitemdown,
        contextmenu,
        shift,
        symbols,
        cursorleft,
        cursorright,
        showtime,
        analogseekforward,
        analogseekback,
        showpreset,
        presetlist,
        nextpreset,
        previouspreset,
        lockpreset,
        randompreset,
        increasevisrating,
        decreasevisrating,
        showvideomenu,
        enter,
        increaserating,
        decreaserating,
        togglefullscreen,
        nextscene,
        previousscene,
        nextletter,
        prevletter,
        jumpsms2,
        jumpsms3,
        jumpsms4,
        jumpsms5,
        jumpsms6,
        jumpsms7,
        jumpsms8,
        jumpsms9,
        filter,
        filterclear,
        filtersms2,
        filtersms3,
        filtersms4,
        filtersms5,
        filtersms6,
        filtersms7,
        filtersms8,
        filtersms9,
        firstpage,
        lastpage,
        guiprofile,
        red,
        green,
        yellow,
        blue,
        increasepar,
        decreasepar,
        volampup,
        volampdown,
        channelup,
        channeldown,
        previouschannelgroup,
        nextchannelgroup,
        leftclick,
        rightclick,
        middleclick,
        doubleclick,
        wheelup,
        wheeldown,
        mousedrag,
        mousemove,
        noop
    }

    public enum GuiWindow
    {
        home,
        programs,
        pictures,
        filemanager,
        files,
        settings,
        music,
        video,
        videos,
        tv,
        pvr,
        pvrguideinfo,
        pvrrecordinginfo,
        pvrtimersetting,
        pvrgroupmanager,
        pvrchannelmanager,
        pvrguidesearch,
        pvrchannelscan,
        pvrupdateprogress,
        pvrosdchannels,
        pvrosdguide,
        pvrosddirector,
        pvrosdcutter,
        pvrosdteletext,
        systeminfo,
        testpattern,
        screencalibration,
        guicalibration,
        picturessettings,
        programssettings,
        weathersettings,
        musicsettings,
        systemsettings,
        videossettings,
        networksettings,
        servicesettings,
        appearancesettings,
        pvrsettings,
        tvsettings,
        scripts,
        videofiles,
        videolibrary,
        videoplaylist,
        loginscreen,
        profiles,
        skinsettings,
        addonbrowser,
        yesnodialog,
        progressdialog,
        virtualkeyboard,
        volumebar,
        submenu,
        favourites,
        contextmenu,
        infodialog,
        numericinput,
        gamepadinput,
        shutdownmenu,
        mutebug,
        playercontrols,
        seekbar,
        musicosd,
        addonsettings,
        visualisationsettings,
        visualisationpresetlist,
        osdvideosettings,
        osdaudiosettings,
        videobookmarks,
        filebrowser,
        networksetup,
        mediasource,
        profilesettings,
        locksettings,
        contentsettings,
        songinformation,
        smartplaylisteditor,
        smartplaylistrule,
        busydialog,
        pictureinfo,
        accesspoints,
        fullscreeninfo,
        karaokeselector,
        karaokelargeselector,
        sliderdialog,
        addoninformation,
        musicplaylist,
        musicfiles,
        musiclibrary,
        musicplaylisteditor,
        teletext,
        selectdialog,
        musicinformation,
        okdialog,
        movieinformation,
        textviewer,
        fullscreenvideo,
        fullscreenlivetv,
        visualisation,
        slideshow,
        filestackingdialog,
        karaoke,
        weather,
        screensaver,
        videoosd,
        videomenu,
        videotimeseek,
        musicoverlay,
        videooverlay,
        startwindow,
        startup,
        peripherals,
        peripheralsettings,
        extendedprogressdialog,
        mediafilter
    }

    public enum ApplicationPropertyName
    {
        volume,
        muted,
        name,
        version
    }

    public enum VideoTypes
    {
        Movie,
        TvShow,
        MusicVideo
    }

    public enum PlaylistType
    {
        unknown,
        video,
        audio,
        picture,
        mixed
    }
}