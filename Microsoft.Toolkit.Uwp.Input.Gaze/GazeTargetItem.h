//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "GazePointerState.h"
#include "GazeApi.h"
#include "GazeProgressState.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

namespace Shapes = Windows::UI::Xaml::Shapes;

BEGIN_NAMESPACE_GAZE_INPUT

ref struct GazeTargetItem sealed
{
    property int ElapsedTime;
    property int NextStateTime;
    property int64 LastTimestamp;
    property GazePointerState ElementState;
    property UIElement^ TargetElement;
    property int RepeatCount;
    property int MaxRepeatCount;

    GazeTargetItem(UIElement^ target)
    {
        TargetElement = target;
    }

    void Reset(int nextStateTime)
    {
        ElementState = GazePointerState::PreEnter;
        ElapsedTime = 0;
        NextStateTime = nextStateTime;
        RepeatCount = 0;
        MaxRepeatCount = GazeApi::GetMaxRepeatCount(TargetElement);
    }

    void GiveFeedback()
    {
        if (_nextStateTime != NextStateTime)
        {
            _prevStateTime = _nextStateTime;
            _nextStateTime = NextStateTime;
        }

        if (ElementState != _notifiedPointerState)
        {
            switch (ElementState)
            {
            case GazePointerState::Dwell:
            case GazePointerState::Fixation:
                RaiseProgressEvent(GazeProgressState::Progressing);
                break;

            case GazePointerState::Exit:
            case GazePointerState::PreEnter:
                RaiseProgressEvent(GazeProgressState::Idle);
                break;
            }

            _notifiedPointerState = ElementState;
        }
        else if (ElementState == GazePointerState::Dwell || ElementState == GazePointerState::Fixation)
        {
            if (RepeatCount <= MaxRepeatCount)
            {
                RaiseProgressEvent(GazeProgressState::Progressing);
            }
            else
            {
                RaiseProgressEvent(GazeProgressState::Complete);
            }
        }
    }

private:

    void RaiseProgressEvent(GazeProgressState state);

    GazePointerState _notifiedPointerState = GazePointerState::Exit;
    int64 _prevStateTime;
    int64 _nextStateTime;
    GazeProgressState _notifiedProgressState = GazeProgressState::Idle;
    Popup^ _feedbackPopup;
};

END_NAMESPACE_GAZE_INPUT