// Copyright (c) 2015 - 2019 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Collections.Generic;
using Doozy.Editor.Nody.NodeGUI;
using Doozy.Engine.Nody.Models;
using UnityEngine;

namespace Doozy.Editor.Nody
{
    [Serializable]
    public class GraphEvent
    {
        private static List<Action<GraphEvent>> s_receivers;
        private static List<Action<GraphEvent>> Receivers { get { return s_receivers ?? (s_receivers = new List<Action<GraphEvent>>()); } }

        public static void AddReceiver(Action<GraphEvent> action)
        {
            if (Receivers.Contains(action)) return;
            Receivers.Add(action);
//            DDebug.Log("Add Receiver: " + action.Method.Name);
        }

        public static void RemoveReceiver(Action<GraphEvent> action)
        {
            if (!Receivers.Contains(action)) return;
            Receivers.Remove(action);
//            DDebug.Log("Remove Receiver: " + action.Method.Name);
        }

        private static void TriggerReceivers(GraphEvent graphEvent)
        {
            if (Receivers.Count == 0) return;
            for (int i = Receivers.Count - 1; i >= 0; i--)
            {
                if (Receivers[i] == null)
                {
                    Receivers.RemoveAt(i);
                    continue;
                }
                Receivers[i].Invoke(graphEvent);
            }
        }

        public static Action<GraphEvent> OnGraphEvent = delegate { };

        public readonly EventType eventType;
        public readonly BaseNodeGUI sourceNodeGUI;
        public readonly Socket sourceSocket;
        public readonly Vector2 eventPosition;
        public readonly Vector2 mousePosition;
        public readonly string message;

        public GraphEvent(EventType eventType) { this.eventType = eventType; }

        public GraphEvent(EventType eventType, string message)
        {
            this.eventType = eventType;
            this.message = message;
        }

        public GraphEvent(EventType eventType, BaseNodeGUI sourceNodeGUI)
        {
            this.eventType = eventType;
            this.sourceNodeGUI = sourceNodeGUI;
        }

        public GraphEvent(EventType eventType, BaseNodeGUI sourceNodeGUI, string message)
        {
            this.eventType = eventType;
            this.sourceNodeGUI = sourceNodeGUI;
            this.message = message;
        }

        private GraphEvent(EventType eventType, Vector2 mousePosition)
        {
            this.eventType = eventType;
            this.mousePosition = mousePosition;
        }

        public GraphEvent(EventType eventType, BaseNodeGUI sourceNodeGUI, Vector2 mousePosition)
        {
            this.eventType = eventType;
            this.sourceNodeGUI = sourceNodeGUI;
            this.mousePosition = mousePosition;
        }

        public GraphEvent(EventType eventType, BaseNodeGUI sourceNodeGUI, Socket sourceSocket, Vector2 mousePosition)
        {
            this.eventType = eventType;
            this.sourceNodeGUI = sourceNodeGUI;
            this.sourceSocket = sourceSocket;
            this.mousePosition = mousePosition;
        }

        public GraphEvent(EventType eventType, BaseNodeGUI sourceNodeGUI, Socket sourceSocket, Vector2 eventPosition, Vector2 mousePosition)
        {
            this.eventType = eventType;
            this.sourceNodeGUI = sourceNodeGUI;
            this.sourceSocket = sourceSocket;
            this.eventPosition = eventPosition;
            this.mousePosition = mousePosition;
        }

        public GraphEvent(EventType eventType, BaseNodeGUI sourceNodeGUI, Socket sourceSocket, Vector2 eventPosition, Vector2 mousePosition, string message)
        {
            this.eventType = eventType;
            this.sourceNodeGUI = sourceNodeGUI;
            this.sourceSocket = sourceSocket;
            this.eventPosition = eventPosition;
            this.mousePosition = mousePosition;
            this.message = message;
        }

        private static void Send(GraphEvent graphEvent)
        {
            OnGraphEvent.Invoke(graphEvent);
            TriggerReceivers(graphEvent);
        }

        public static void Send(EventType eventType) { Send(new GraphEvent(eventType)); }
        public static void Send(EventType eventType, string message) { Send(new GraphEvent(eventType, message)); }
        public static void Send(EventType eventType, BaseNodeGUI sourceNodeGUI) { Send(new GraphEvent(eventType, sourceNodeGUI)); }
        public static void Send(EventType eventType, BaseNodeGUI sourceNodeGUI, string message) { Send(new GraphEvent(eventType, sourceNodeGUI, message)); }
        public static void Send(EventType eventType, Vector2 mousePosition) { Send(new GraphEvent(eventType, mousePosition)); }
        public static void Send(EventType eventType, BaseNodeGUI sourceNodeGUI, Vector2 mousePosition) { Send(new GraphEvent(eventType, sourceNodeGUI, mousePosition)); }
        public static void Send(EventType eventType, BaseNodeGUI sourceNodeGUI, Socket sourceSocket, Vector2 mousePosition) { Send(new GraphEvent(eventType, sourceNodeGUI, sourceSocket, mousePosition)); }
        public static void Send(EventType eventType, BaseNodeGUI sourceNodeGUI, Socket sourceSocket, Vector2 eventPosition, Vector2 mousePosition) { Send(new GraphEvent(eventType, sourceNodeGUI, sourceSocket, eventPosition, mousePosition)); }
        public static void Send(EventType eventType, BaseNodeGUI sourceNodeGUI, Socket sourceSocket, Vector2 eventPosition, Vector2 mousePosition, string message) { Send(new GraphEvent(eventType, sourceNodeGUI, sourceSocket, eventPosition, mousePosition, message)); }

        public enum EventType
        {
            EVENT_NONE,

            EVENT_CONNECTING_BEGIN,
            EVENT_CONNECTING_END,
            EVENT_CONNECTION_ESTABLISHED,

            EVENT_NODE_CREATED,
            EVENT_NODE_DELETED,
            EVENT_NODE_UPDATED,
            EVENT_NODE_CLICKED,
            EVENT_NODE_DISCONNECTED,

            EVENT_SOCKET_CREATED,
            EVENT_SOCKET_ADDED,
            EVENT_SOCKET_REMOVED,
            EVENT_SOCKET_CLEARED_CONNECTIONS,

            EVENT_CONNECTION_CREATED,
            EVENT_CONNECTION_TAPPED,
            EVENT_CONNECTION_DELETED,

            EVENT_RECORD_UNDO,
            EVENT_UNDO_REDO_PERFORMED,
            EVENT_SAVED_ASSETS,
            
            EVENT_GRAPH_OPENED
        }
    }
}