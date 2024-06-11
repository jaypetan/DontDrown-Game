from mlagents_envs.demonstrations import load_demonstration

demonstration_path = "C:\\Users\\zongj\\OneDrive - Nanyang Technological University\\NTU\\School\\URECA\\DontDrown\\SubmarineGame\\Assets\\Demo\\SharkAgent.demo"
demo_buffer, meta_info = load_demonstration(demonstration_path)
print(f"Number of episodes: {meta_info['number_of_episodes']}")
print(f"Number of steps: {meta_info['number_of_steps']}")
