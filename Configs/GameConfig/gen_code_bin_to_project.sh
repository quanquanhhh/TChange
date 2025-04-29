#!/bin/bash

cd "$(dirname "$0")"
echo "当前目录: $(pwd)"

export WORKSPACE="$(realpath ../../)"
export LUBAN_DLL="${WORKSPACE}/Tools/Luban/Luban.dll"
export CONF_ROOT="$(pwd)"
export DATA_OUTPATH="${WORKSPACE}/UnityProject/Assets/AssetRaw/Configs/bytes/"
export CODE_OUTPATH="${WORKSPACE}/UnityProject/Assets/GameScripts/HotFix/GameProto/GameConfig/"

cp -R "${CONF_ROOT}/CustomTemplate/ConfigSystem.cs" \
   "${WORKSPACE}/UnityProject/Assets/GameScripts/HotFix/GameProto/ConfigSystem.cs"

dotnet "${LUBAN_DLL}" \
    -t client \
    -c cs-bin \
    -d bin \
    --conf "${CONF_ROOT}/luban.conf" \
    -x outputCodeDir="${CODE_OUTPATH}" \
    -x outputDataDir="${DATA_OUTPATH}"

echo "操作完成，按任意键退出..."
read -k1
