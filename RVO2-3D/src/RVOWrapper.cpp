#include "RVOWrapper.h"

RVO::RVOSimulator* CreateSimulator()
{
	return new RVO::RVOSimulator();
}

void ReleaseSimulator(RVO::RVOSimulator* sim)
{
	delete sim;
}

void SetTimeStep(RVO::RVOSimulator* sim, float timeStep)
{
	if (sim != nullptr)
	{
		sim->setTimeStep(timeStep);
	}
}

void SetAgentDefaults(RVO::RVOSimulator* sim, float neighborDist,
	size_t maxNeighbors, float timeHorizon, float radius,
	float maxSpeed, const RvoVector3& velocity)
{
	if (sim != nullptr)
	{
		sim->setAgentDefaults(neighborDist,
			maxNeighbors, timeHorizon, radius,
			maxSpeed,
			RVO::Vector3(velocity.x, velocity.y, velocity.z));
	}
}

size_t AddAgent(RVO::RVOSimulator* sim, const RvoVector3& position,
	float raduis,
	bool isStatic)
{
	size_t ret = 0;
	if (sim != nullptr)
	{
		ret = sim->addAgent(RVO::Vector3(position.x, position.y, position.z),
			isStatic);
		sim->setAgentRadius(ret, raduis);
	}
	return ret;
}

size_t GetNumAgents(RVO::RVOSimulator* sim)
{
	size_t ret = 0;
	if (sim != nullptr)
	{
		ret = sim->getNumAgents();
	}

	return ret;
}

bool GetAgentPosition(RvoVector3& pos, RVO::RVOSimulator* sim, size_t agentNo)
{
	if (sim != nullptr)
	{
		auto p = sim->getAgentPosition(agentNo);
		pos.x = p.x();
		pos.y = p.y();
		pos.z = p.z();

		return true;
	}

	return false;
}

bool GetAgentVelocity(RvoVector3& velocity, RVO::RVOSimulator* sim, size_t agentNo)
{
	if (sim != nullptr)
	{
		auto v = sim->getAgentVelocity(agentNo);
		velocity.x = v.x();
		velocity.y = v.y();
		velocity.z = v.z();

		return true;
	}

	return false;
}

bool GetAgentRadius(float &radius, RVO::RVOSimulator* sim, size_t agentNo)
{
	if (sim != nullptr)
	{
		radius = sim->getAgentRadius(agentNo);
		return true;
	}

	return false;
}

void SetAgentPrefVelocity(RVO::RVOSimulator* sim, size_t agentNo, const RvoVector3& prefVelocity)
{
	if (sim != nullptr)
	{
		sim->setAgentPrefVelocity(agentNo,
			RVO::Vector3(prefVelocity.x, prefVelocity.y, prefVelocity.z));
	}
}

void SetAgentVelocity(RVO::RVOSimulator* sim, size_t agentNo, const RvoVector3& velocity)
{
	if (sim != nullptr)
	{
		sim->setAgentVelocity(agentNo,
			RVO::Vector3(velocity.x, velocity.y, velocity.z));
	}
}

void DoStep(RVO::RVOSimulator* sim)
{
	if (sim != nullptr)
	{
		sim->doStep();
	}
}
