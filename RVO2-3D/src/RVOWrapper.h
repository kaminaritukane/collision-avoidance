#pragma once

#include "RVO.h"

struct RvoVector3
{
public:
	float x;
	float y;
	float z;

	RvoVector3()
		:x(0),y(0),z(0)
	{
		
	}
};

extern "C" __declspec(dllexport)
RVO::RVOSimulator * CreateSimulator();

extern "C" __declspec(dllexport)
void ReleaseSimulator(RVO::RVOSimulator* sim);

extern "C" __declspec(dllexport)
void SetTimeStep(RVO::RVOSimulator * sim, float timeStep);

extern "C" __declspec(dllexport)
void SetAgentDefaults(RVO::RVOSimulator * sim,
	float neighborDist, size_t maxNeighbors,
	float timeHorizon, float radius, float maxSpeed,
	const RvoVector3 & velocity = RvoVector3());

extern "C" __declspec(dllexport)
size_t AddAgent(RVO::RVOSimulator * sim, const RvoVector3& position,
	float radius,
	bool isStatic = false);

extern "C" __declspec(dllexport)
size_t GetNumAgents(RVO::RVOSimulator * sim);

extern "C" __declspec(dllexport)
bool GetAgentPosition(RvoVector3& pos, RVO::RVOSimulator * sim, size_t agentNo);

extern "C" __declspec(dllexport)
bool GetAgentVelocity(RvoVector3 & velocity, RVO::RVOSimulator * sim, size_t agentNo);

extern "C" __declspec(dllexport)
bool GetAgentRadius(float& radius, RVO::RVOSimulator* sim, size_t agentNo);

extern "C" __declspec(dllexport)
void SetAgentPrefVelocity(RVO::RVOSimulator * sim, size_t agentNo, const RvoVector3 & prefVelocity);

extern "C" __declspec(dllexport)
void SetAgentVelocity(RVO::RVOSimulator * sim, size_t agentNo, const RvoVector3 & velocity);

extern "C" __declspec(dllexport)
void DoStep(RVO::RVOSimulator * sim);
