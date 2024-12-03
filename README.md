# Project2-1
Project 2-1 AI&amp;ML Group 10
## **Project Highlights**

1. **Decoupling Vision from Movement**  
   - Vision is separated from the agent's body movement by linking a Vision script to a standalone component. This allows the agent to move freely without affecting its field of view while ensuring vertical locking of vision rays.

2. **Adding Memory for Contextual Understanding**  
   - Agents remember previous states by storing and appending object positions observed in past frames to current observations, enabling better decision-making based on context.

3. **Hearing Sensor Implementation**  
   - A spherical detection system simulates hearing. Objects entering the sensor’s range are detected, and their positions are recorded, providing agents with auditory input.

4. **Adjusting Field of View**  
   - Backward-facing rays (Blue/Purple-ReverseRays) were removed to mimic realistic visual limitations, ensuring agents only perceive objects in their forward field of view.

---

## **Features**

- **Custom Sensors:** Vision, memory, and hearing-based enhancements for improved decision-making.
- **Realistic Field of Vision:** Backward rays removed for forward-facing visual constraints.
- **Performance Optimization:** Leveraging memory and auditory data without significant computational overhead.

---

## **Getting Started**

### **Prerequisites**
To run this project, ensure the following are installed:
- **Unity Hub:** Version 2022.3 LTS or later recommended.
- **Python:** Version 3.10.12 or higher.
- **ML-Agents Toolkit:** Compatible with version 0.30.0 or higher.
- Required Python libraries: `tensorflow`, `numpy`, `matplotlib`, `gym`.

---

### **Installation**

#### **Step 1: Clone the Repository**
```bash
git clone https://github.com/YourRepositoryLinkHere.git
cd Project2-1
```

#### **Step 2: Unity Setup**
1. Launch Unity Hub and add the project directory.
2. Open the project with Unity Editor version 2022.3 or later.
3. Navigate to **Window > Package Manager**, search for "ML-Agents," and install the latest available version.

#### **Step 3: Python Setup**
1. Create a new Conda environment:
   ```bash
   conda create -n mlagents python=3.10.12 -y
   conda activate mlagents
   ```
2. Install required packages:
   ```bash
   pip install mlagents
   ```
3. Verify installation:
   ```bash
   mlagents-learn --version
   ```

---

### **Project Components**

1. **Vision System**  
   - Decouples vision from body movement, allowing independent navigation. Rays are locked vertically for consistent observations.

2. **Memory Integration**  
   - Observations from previous frames are stored and combined with current frames, enabling agents to make context-aware decisions.

3. **Hearing Sensor**  
   - Detects objects entering a spherical sensor range and records their positions for auditory-based decision-making.

4. **Field of View Adjustment**  
   - Backward rays were removed from the agent prefabs to limit vision to forward-facing observations.

---

## **Training the Agents**

1. **Environment Preparation**
   - Open the `SoccerTwos.unity` scene located in **Assets > Examples > Soccer > Scenes**.
   - Attach the `CustomHearingSensor` and `VisionSystem` scripts to the SoccerAgent prefab if not already configured.

2. **Training Process**
   ```bash
   mlagents-learn --run-id=test1
   ```
   Replace `test1` with a custom run ID to identify your session. Ensure Unity is running and connected to the ML-Agents API.

---

## **Advanced Installation Options**

### **ML-Agents Toolkit Components**
The ML-Agents Toolkit includes:
- Unity Package: `com.unity.ml-agents` (C# SDK integrated into Unity).
- Python Package: `mlagents` (machine learning algorithms for behavior training).
- Example environments for experimentation.

To install locally for development, clone the repository and follow these steps:
```bash
git clone --branch release_21 https://github.com/Unity-Technologies/ml-agents.git
```

---

## **Next Steps**

1. Experiment with **multi-agent environments** and larger teams.
2. Optimize **training configurations** (e.g., learning rate, neural network size) for quicker convergence.
3. Evaluate performance metrics using Unity Profiler.

---

## **Team Contributions**

| Name                  | 
|-----------------------|
| **Deniz Derviş**      | 
| **Dimitrios Tsiplakis**| 
| **Adrian Rusu**       | 
| **Aleksandr Voloshin**| 
| **Max Nicolai**       | 
| **Amir Kalantarzadeh**| 
| **Alen Quiroz Engel** | 

---

