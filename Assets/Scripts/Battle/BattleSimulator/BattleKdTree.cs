/*
 * RVO Library / RVO2 Library
 * 
 * Copyright © 2008-10 University of North Carolina at Chapel Hill. All rights 
 * reserved.
 * 
 * Permission to use, copy, modify, and distribute this software and its 
 * documentation for educational, research, and non-profit purposes, without 
 * fee, and without a written agreement is hereby granted, provided that the 
 * above copyright notice, this paragraph, and the following four paragraphs 
 * appear in all copies.
 * 
 * Permission to incorporate this software into commercial products may be 
 * obtained by contacting the University of North Carolina at Chapel Hill.
 * 
 * This software program and documentation are copyrighted by the University of 
 * North Carolina at Chapel Hill. The software program and documentation are 
 * supplied "as is", without any accompanying services from the University of 
 * North Carolina at Chapel Hill or the authors. The University of North 
 * Carolina at Chapel Hill and the authors do not warrant that the operation of 
 * the program will be uninterrupted or error-free. The end-user understands 
 * that the program was developed for research purposes and is advised not to 
 * rely exclusively on the program for any reason.
 * 
 * IN NO EVENT SHALL THE UNIVERSITY OF NORTH CAROLINA AT CHAPEL HILL OR ITS 
 * EMPLOYEES OR THE AUTHORS BE LIABLE TO ANY PARTY FOR DIRECT, INDIRECT, 
 * SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES, INCLUDING LOST PROFITS, 
 * ARISING OUT OF THE USE OF THIS SOFTWARE AND ITS DOCUMENTATION, EVEN IF THE 
 * UNIVERSITY OF NORTH CAROLINA AT CHAPEL HILL OR THE AUTHORS HAVE BEEN ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * THE UNIVERSITY OF NORTH CAROLINA AT CHAPEL HILL AND THE AUTHORS SPECIFICALLY 
 * DISCLAIM ANY WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE AND ANY 
 * STATUTORY WARRANTY OF NON-INFRINGEMENT. THE SOFTWARE PROVIDED HEREUNDER IS ON 
 * AN "AS IS" BASIS, AND THE UNIVERSITY OF NORTH CAROLINA AT CHAPEL HILL AND THE 
 * AUTHORS HAVE NO OBLIGATIONS TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, 
 * ENHANCEMENTS, OR MODIFICATIONS.
 */

using System.Collections.Generic;
using System;

namespace Battle
{
    internal class BattleKdTree
    {
//        private struct FloatPair
//        {
//            internal float _a;
//            internal float _b;
//
//            public FloatPair(float a, float b)
//            {
//                _a = a;
//                _b = b;
//            }
//
//            public static bool operator <(FloatPair lhs, FloatPair rhs)
//            {
//                return (lhs._a < rhs._a || !(rhs._a < lhs._a) && lhs._b < rhs._b);
//            }
//            public static bool operator <=(FloatPair lhs, FloatPair rhs)
//            {
//                return (lhs._a == rhs._a && lhs._b==rhs._b) || lhs < rhs;
//            }
//            public static bool operator >(FloatPair lhs, FloatPair rhs)
//            {
//                return !(lhs<=rhs);
//            }
//            public static bool operator >=(FloatPair lhs, FloatPair rhs)
//            {
//                return !(lhs < rhs);
//            }
//        }
//
//		internal BattleSimulator simulator;
//        private const int MAX_LEAF_SIZE = 2;
//
//        private struct AgentTreeNode
//        {
//            internal int begin;
//            internal int end;
//            internal int left;
//            internal float maxX;
//            internal float maxY;
//            internal float minX;
//            internal float minY;
//            internal int right;
//        }
//
//        //private IList<Agent> agents_ = new List<Agent>();
//        //private IList<AgentTreeNode> agentTree_ = new List<AgentTreeNode>();
//        private BattleAgent[] agents_;
//        private AgentTreeNode[] agentTree_;
//
//
//        internal void buildAgentTree()
//        {
//            if (agents_==null || agents_.Length != simulator.agents.Count)
//            {
//                agents_ = new BattleAgent[simulator.agents.Count];
//                for (int i = 0; i < agents_.Length; ++i)
//                {
//                    agents_[i] = simulator.agents[i];
//                }
//
//                agentTree_ = new AgentTreeNode[2 * agents_.Length];
//                for (int i = 0; i < agentTree_.Length; ++i)
//                {
//                    agentTree_[i] = new AgentTreeNode();
//                }
//            }
//
//            if (agents_.Length != 0)
//            {
//                buildAgentTreeRecursive(0, agents_.Length, 0);
//            }
//        }
//
//        void buildAgentTreeRecursive(int begin, int end, int node)
//        {
//            agentTree_[node].begin = begin;
//            agentTree_[node].end = end;
//            agentTree_[node].minX = agentTree_[node].maxX = agents_[begin].position_.x_;
//            agentTree_[node].minY = agentTree_[node].maxY = agents_[begin].position_.y_;
//            
//            for (int i = begin + 1; i < end; ++i)
//            {
//                agentTree_[node].maxX = Math.Max(agentTree_[node].maxX, agents_[i].position_.x_);
//                agentTree_[node].minX = Math.Min(agentTree_[node].minX, agents_[i].position_.x_);
//                agentTree_[node].maxY = Math.Max(agentTree_[node].maxY, agents_[i].position_.y_);
//                agentTree_[node].minY = Math.Min(agentTree_[node].minY, agents_[i].position_.y_);
//            }
//            
//            if (end - begin > MAX_LEAF_SIZE)
//            {
//                /* No leaf node. */
//                bool isVertical = (agentTree_[node].maxX - agentTree_[node].minX > agentTree_[node].maxY - agentTree_[node].minY);
//                float splitValue = (isVertical ? 0.5f * (agentTree_[node].maxX + agentTree_[node].minX) : 0.5f * (agentTree_[node].maxY + agentTree_[node].minY));
//                
//                int left = begin;
//                int right = end;
//                
//                while (left < right)
//                {
//                    while (left < right && (isVertical ? agents_[left].position_.x_ : agents_[left].position_.y_) < splitValue)
//                    {
//                        ++left;
//                    }
//                    
//                    while (right > left && (isVertical ? agents_[right - 1].position_.x_ : agents_[right - 1].position_.y_) >= splitValue)
//                    {
//                        --right;
//                    }
//                    
//                    if (left < right)
//                    {
//                        BattleAgent tmp = agents_[left];
//                        agents_[left] = agents_[right - 1];
//                        agents_[right - 1] = tmp;
//                        ++left;
//                        --right;
//                    }
//                }
//                
//                int leftSize = left - begin;
//                
//                if (leftSize == 0)
//                {
//                    ++leftSize;
//                    ++left;
//                    ++right;
//                }
//                
//                agentTree_[node].left = node + 1;
//                agentTree_[node].right = node + 1 + (2 * leftSize - 1);
//                
//                buildAgentTreeRecursive(begin, left, agentTree_[node].left);
//                buildAgentTreeRecursive(left, end, agentTree_[node].right);
//            }
//        }
//
//        internal void computeAgentNeighbors(BattleAgent agent, ref float rangeSq)
//        {
//            queryAgentTreeRecursive(agent, ref rangeSq, 0);
//        }
//
//        void queryAgentTreeRecursive(BattleAgent agent, ref float rangeSq, int node)
//        {
//            if (agentTree_[node].end - agentTree_[node].begin <= MAX_LEAF_SIZE)
//            {
//                for (int i = agentTree_[node].begin; i < agentTree_[node].end; ++i)
//                {
//                    agent.insertAgentNeighbor(agents_[i], ref rangeSq);
//                }
//            }
//            else
//            {
//                float distSqLeft = BattleMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].left].minX - agent.position_.x_)) + BattleMath.sqr(Math.Max(0.0f, agent.position_.x_ - agentTree_[agentTree_[node].left].maxX)) + BattleMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].left].minY - agent.position_.y_)) + BattleMath.sqr(Math.Max(0.0f, agent.position_.y_ - agentTree_[agentTree_[node].left].maxY));
//                
//                float distSqRight = BattleMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].right].minX - agent.position_.x_)) + BattleMath.sqr(Math.Max(0.0f, agent.position_.x_ - agentTree_[agentTree_[node].right].maxX)) + BattleMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].right].minY - agent.position_.y_)) + BattleMath.sqr(Math.Max(0.0f, agent.position_.y_ - agentTree_[agentTree_[node].right].maxY));
//                
//                if (distSqLeft < distSqRight)
//                {
//                    if (distSqLeft < rangeSq)
//                    {
//                        queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].left);
//                        
//                        if (distSqRight < rangeSq)
//                        {
//                            queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].right);
//                        }
//                    }
//                }
//                else
//                {
//                    if (distSqRight < rangeSq)
//                    {
//                        queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].right);
//                        
//                        if (distSqLeft < rangeSq)
//                        {
//                            queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].left);
//                        }
//                    }
//                }
//                
//            }
//        }
    }
}