import torch

if torch.cuda.is_available():
    device = torch.device('cuda')
    print("CUDA is available. Using GPU:", torch.cuda.get_device_name(device))
else:
    device = torch.device('cpu')
    print("CUDA is not available. Using CPU instead.")
