﻿namespace Stack
{
    public class Node
    {
        public int value;
        public Node next;
        public Node prev;
        public Node(int _value)
        {
            value = _value;
            next = null;
            prev = null;
        }

        public int GetValue()
        {
            return value;
        }
    }

    public class Stack
    {
        public Node head;
        public Node tail;

        public Stack()
        {
            head = null;
            tail = null;
        }

        public int Size()
        {
            int count = 0;
            Node node = head;
            while (node != null)
            {
                node = node.next;
                count++;
            }
            return count;
        }

        public void Push(Node _item)
        {

            if (head == null)
            {
                head = _item;
                _item.prev = null;
                _item.next = null;
            }
            else
            {
                tail.next = _item;
                _item.prev = tail;
            }

            tail = _item;
        }

        public Node Pop()
        {
            Node lastNode = tail;
            if (tail != null)
            {

                lastNode.prev.next = null;
                tail = lastNode.prev;
                return lastNode;
            }
            return null;
        }

        public Node Peek()
        {
            Node lastNode = tail;
            if (tail != null)
                return lastNode;
            return null;
        }
    }
}